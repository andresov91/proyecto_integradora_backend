using AutoMapper;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Resources;

using Microsoft.AspNetCore.Mvc;
using kairosApp.Domain.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;
using kairosApp.Helpers;
using System.Diagnostics;
using kairosApp.Resources.Support;
using kairosApp.Models.Support;
using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Models.Support.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace kairosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaUsuarioController : ControllerBase
    {
        private readonly ICuentaUsuarioService _cuentaUsuarioService;
        private readonly IMapper _mapper;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;

        private IEnumerable<CuentaUsuario> logins = new List<CuentaUsuario>() {
            new CuentaUsuario() {
                    Id = 1,
                        Alias = "carlos.salazar",
                        Username = "carfesal",
                },
                new CuentaUsuario() {
                    Id = 2,
                        Alias = "melanie.banchon",
                        Username = "meldaban",
                }
        };
        public CuentaUsuarioController(ICuentaUsuarioService cuentaUsuarioService, IMapper mapper, IActiveDirectoryService activeDirectoryService, JwtSettings jwtSettings, AppDbContext context, IEmailSender emailSender)
        {
            _cuentaUsuarioService = cuentaUsuarioService;
            _mapper = mapper;
            _activeDirectoryService = activeDirectoryService;
            _jwtSettings = jwtSettings;
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IEnumerable<CuentaUsuarioResource>> GetAllAsync()
        {
            var accounts = await _cuentaUsuarioService.ListAsync();
            foreach (var account in accounts)
            {
                Debug.WriteLine(account.Username+" "+account.PersonaId);
            }
            var rsc = _mapper.Map<IEnumerable<CuentaUsuario>, IEnumerable<CuentaUsuarioResource>>(accounts);
            var retornable = rsc.Where(p => p.IsActive == true);
            return retornable;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CrearCuentaResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            var cuentaRsc = new SaveCuentaUsuarioResource { Alias = resource.Alias, IsActive = true, Username = resource.Username, PersonaId=resource.PersonaId};
            var cuenta = _mapper.Map<SaveCuentaUsuarioResource, CuentaUsuario>(cuentaRsc);
            
            var persona = _context.Personas.FirstOrDefault(p => p.Id == resource.PersonaId);
            if(persona == null) { return BadRequest(new ErrorResource { ErrorMessage = "Ocurrio un error" }); }
            var result = await _cuentaUsuarioService.SaveAsync(cuenta);
            
            if (!result.Success)
                return BadRequest(result.Message);

            var resultAD = _activeDirectoryService.CreateUser(new ADCreateUser
            {
                Username = resource.Username,
                Persona = new Persona { 
                    Nombres = persona.Nombres, 
                    Apellidos = persona.Apellidos,
                    Rol = persona.Rol,
                    Unidad = persona.Unidad,
                    Telefono = persona.Telefono,
                    CorreoAlterno = persona.CorreoAlterno,
                    Identificacion = persona.Identificacion
                },
                Password = resource.Password
            });

            if (resultAD)
            {
                var cuentaResource = _mapper.Map<CuentaUsuario, CuentaUsuarioResource>(result.CuentaUsuario);
                return Ok(cuentaResource);
            }
            return BadRequest(new ErrorResource { ErrorMessage = "Ocurrio un problema al crear el usuario"});
            
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCuentaUsuarioResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var cuenta = _mapper.Map<SaveCuentaUsuarioResource, CuentaUsuario>(resource);
            var result = await _cuentaUsuarioService.UpdateAsync(id, cuenta);

            if (!result.Success)
                return BadRequest(result.Message);

            var cuentaUsuarioResource = _mapper.Map<CuentaUsuario, CuentaUsuarioResource>(result.CuentaUsuario);
            return Ok(cuentaUsuarioResource);
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserCredentials credentials)
        {
            try
            {
                var Token = new UserTokens();
                var Valid = _context.CuentaUsuarios.Any(x => x.Username == credentials.Username);
                if (Valid)
                {
                    var user = _context.CuentaUsuarios.FirstOrDefault(x => x.Username == credentials.Username);
                    if (user == null) { return NotFound(new ErrorResource { ErrorMessage = "Usuario no encontrado" }); }
                    
                    ADToDBUser userAD = _activeDirectoryService.Login(credentials.Username, credentials.Password);
                    if(userAD == null) { return BadRequest(new ErrorResource { ErrorMessage = "Usuario y/o Contraseña inocrrectos" }); }

                    var persona = _context.Personas.FirstOrDefault(x => x.Id == user.PersonaId);
                    Debug.WriteLine(persona.Nombres);
                    Token = JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        Username = user.Username,
                        Id = user.Id,
                        Persona = persona,
                    }, _jwtSettings);
                }
                else
                {
                    ADToDBUser ADUser = _activeDirectoryService.Login(credentials.Username, credentials.Password);
                    if(ADUser != null)
                    {
                        Debug.WriteLine("Entra al proceso de AD");
                        Persona persona = ADUser.Persona;
                        _context.Personas.Add(persona);
                        _context.SaveChanges();
                        CuentaUsuario cu = ADUser.CuentaUsuario;
                        cu.IsActive = true;
                        cu.PersonaId = persona.Id;
                        _context.CuentaUsuarios.Add(cu);
                        _context.SaveChanges();
                        Token = JwtHelpers.GenTokenkey(new UserTokens()
                        {
                            Username = cu.Username,
                            Id = cu.Id,
                            Persona = persona,
                        }, _jwtSettings);
                        
                    }
                    else
                        return NotFound(new ErrorResource { ErrorMessage = "Usuario y/o Contraseña incorrectos" });

                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest(new ErrorResource { ErrorMessage = "Ocurrio un error: "+ex.Message });
            }

        }
        [HttpPost]
        [Route("pruebasEndpoints")]
        public async Task<IActionResult> pruebas([FromBody] UserCredentials credentials)
        {
            bool info = false;
            //CODIGO DEL ACTIVE DIRECTORY SERVICE
            //_activeDirectoryService.GetAUser("fernsala");//carzamch
            //var respuesta = _activeDirectoryService.FindUserByIdentification("0910847805");
            //_activeDirectoryService.GetAdditionalUserInfo();
            //var respuesta = _activeDirectoryService.Login("carpacino", "fweoSD654");
            //var respuesta = _activeDirectoryService.CreateUser(new ADCreateUser { Persona = new Persona { Nombres = "Carlos Emilio", Apellidos = "Zamora Chinchipe", Identificacion = "0904475969", Telefono = "0991193877", Rol ="Estudiante", Unidad="FIEC",CorreoAlterno="carlosemi123515@hotmail.com"}, Username = "carpacino"});
            var respuesta = _activeDirectoryService.ResetPassword("fernsala", "Carlitos123");
            //Debug.WriteLine("Se creo el usuario: "+ respuesta);
            return Ok("Contraseña cambiada exitosamente");
        }

        [HttpPost]
        [Route("change")]
        public async Task<IActionResult> ChangePassword([FromBody] UserCredentials credentials)
        {
            var verifyUser = _activeDirectoryService.GetAUser(credentials.Username);
            if(verifyUser == "Encontrado")
            {
                //Contra de prueba= Carlitos123
                var cambioContra = _activeDirectoryService.ResetPassword(credentials.Username, credentials.Password);
                if (cambioContra)
                {
                    return Ok(new ResponseResource { Success = true, Message = "Contraseña cambia exitosamente" });
                }
                return BadRequest(new ErrorResource { ErrorMessage = "Ocurrio un error al cambiar la contraseña." });
            }
            return Ok(new ResponseResource { Success = false, Message = "Usuario no encontrado en AD" });
        }

        
        [HttpPost]
        [Route("verificaremail")]
        public async Task<IActionResult> ResetPassword ( [FromBody] PersonResetPasswordCredentials credentials)
        {
            var respuesta = _cuentaUsuarioService.VerifyEmail(credentials);
            //var respuestaAD = _activeDirectoryService.verifyUsername();
            if (respuesta)
            {
                try
                {
                    var newPassWord = _cuentaUsuarioService.CreateNewPassword();
                    /** AQUI SE ENVIA EL MENSAJE **/
                    var message = new Message(new string[] { credentials.Correo }, "Cambio de contraseña", "La nueva contraseña para su cuenta es: " + newPassWord);
                    _emailSender.SendEmail(message);
                    /***/
                    var respuestaAD = _activeDirectoryService.ResetPassword(credentials.Username, newPassWord);
                    if (respuestaAD)
                    {
                        //Se envia mensaje al correo
                        Debug.WriteLine("Contraseña reseteada a: " + newPassWord);
                        
                        return Ok(new ResponseResource { Success = true, Message = "Contraseña reseteada con exito." });
                    }
                    return NotFound(new ErrorResource { ErrorMessage = "No se ha encontrado al usuario" });
                    
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorResource { ErrorMessage = "Ha ocurrido un problema: " + ex.Message });
                }
                
            }
            return NotFound(new ErrorResource { ErrorMessage = "Correo alterno incorrecto." });



        }
        [HttpGet]
        [Route("verificaralias/{alias}")]
        public async Task<IActionResult> verifyAlias(string alias)
        {
            var respuesta = _cuentaUsuarioService.VerifyAlias(alias);
            if (respuesta)
            {
                return Ok(new ResponseResource { Success = respuesta, Message = "Alias ya existente"});
            }
            return Ok(new ResponseResource { Success = respuesta, Message = "Alias no existe en base" });
        }

        [HttpGet]
        [Route("verificarusername/{username}")]
        public async Task<IActionResult> verifyUsername(string username)
        {
            var respuesta = _cuentaUsuarioService.VerifyUsername(username);
            var respuestaAD = _activeDirectoryService.GetAUser(username);
            var bandera = respuestaAD == "Encontrado" ? true : false;

            if (respuesta || bandera)
            {
                return Ok(new ResponseResource { Success = respuesta, Message = "Username ya existente" });
            }
            return Ok(new ResponseResource { Success = respuesta, Message = "Username no existe en base" });
        }
    }
    

}
