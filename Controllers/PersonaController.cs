using AutoMapper;
using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Models.Support;
using kairosApp.Resources;
using kairosApp.Resources.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistroCivilService;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace kairosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaService _personaService;
        private readonly IMapper _mapper;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly AppDbContext _context;
        private readonly IRGService _rGService;
        public PersonaController(IPersonaService personaService, IMapper mapper, IActiveDirectoryService activeDirectoryService, AppDbContext context, IRGService rGService)
        {
            _personaService = personaService;
            _mapper = mapper;
            _activeDirectoryService = activeDirectoryService;
            _context = context;
            _rGService = rGService;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonaResource>> GetAllAsync()
        {
            var people = await _personaService.ListAsync();
            var rsc = _mapper.Map<IEnumerable<Persona>, IEnumerable<PersonaResource>>(people);

            return rsc;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SavePersonaResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            
            var persona = _mapper.Map<SavePersonaResource, Persona>(resource);

            var result = await _personaService.SaveAsync(persona);

            if (!result.Success)
                return BadRequest(result.Message);

            var personaResource = _mapper.Map<Persona, PersonaResource>(result.Persona);
            return Ok(personaResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SavePersonaResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var persona = _mapper.Map<SavePersonaResource, Persona>(resource);
            var result = await _personaService.UpdateAsync(id, persona);

            if (!result.Success)
                return BadRequest(result.Message);

            var personaResource = _mapper.Map<Persona, PersonaResource>(result.Persona);
            return Ok(personaResource);
        }

        private async Task<ciudadano> GetCiudadano(string identificacion)
        {
            ciudadano respuesta;
            MethodInfo method = typeof(XmlSerializer).GetMethod("set_Mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { 1 });

            wsBSGProxySoapClient clientws = new wsBSGProxySoapClient(0);
            AuthSoapHeaders header = new AuthSoapHeaders();
            header.usr = "webacad";
            header.pwd = "Ch54ai3se";

            try
            {
                GetCiudadanoResponse data = await clientws.GetCiudadanoAsync(header, identificacion);
                respuesta = data.GetCiudadanoResult;
            }
            catch { respuesta = null; }

            return respuesta;
        }
        
        [HttpGet]
        [Route("pruebas/{identificacion:minlength(10)}/{fecha}/{tipo}")]
        public async Task<IActionResult> pruebas(string identificacion,string fecha, string tipo)
        {
            //var respuesta = await _rGService.VerifyIdentification(identificacion, fecha, tipo);
            var respuesta = await _rGService.ValidateIdentificationNumber(identificacion);
            return Ok("Todo belen mi llave: "+respuesta);
        }

        [HttpGet]
        [Route("{identificacion:minlength(10)}/{fecha}/{tipo}")]
        public async Task<IActionResult> GetPerson(string identificacion, string fecha, string tipo) 
        {
            var cedulaVerified = await _rGService.VerifyIdentification(identificacion, fecha, tipo);
            //Se verifica que los datos de la identificacion sean correctos caso contrario se envia un error
            if (cedulaVerified == null) { return BadRequest(new ErrorResource { ErrorMessage = "Datos de identificacion erroneos."}); }            
            
            //Se busca a la persona con esa cedula en el AD 
            var respuestaAD = _activeDirectoryService.FindUserByIdentification(identificacion);

            //Se busca a la persona con esa cedula en la base local
            var persona = _personaService.GetPersonaByCedula(identificacion);
            Debug.WriteLine("Respuesta AD: " + respuestaAD); 
            if (respuestaAD == null) 
            {
                //En el caso de no encontrar a la persona en el AD se valida si la persona esta en la base local
                //En caso de no estar se envia 
                if (persona == null) 
                { 
                    //Si no se encuentra persona en la base local se devuelve la informacion de la persona obtenida del servicio
                    //del Registro Civil
                    return Ok(
                        new ResponsePersona { 
                            Success = false, 
                            Persona = new PersonaCuentaResource { 
                                Persona = new Persona { 
                                    Identificacion = identificacion, 
                                    Nombres = cedulaVerified.Nombres, 
                                    Apellidos = cedulaVerified.Apellidos }, 
                                Usuarios = null } }); 
                }
                if (persona.Persona == null) { return Ok(new ResponsePersona { Success = false, Persona = null }); }
                return Ok(new ResponsePersona { Success = true, Persona = persona });
            }
            return Ok(new ResponsePersona { Success = false, Persona = null });
            //ACCION A LA OTRA BASE DE DATOS 
            /*if (persona == null) { return NotFound(new ErrorResource { ErrorMessage = "No se encontro persona con esa cedula."}); }
            if( persona.Persona == null) { return Ok(new ResponsePersona { Success = false, Persona = null}); }*/

        }

        [HttpGet]
        [Route("cuenta/{identificacion:minlength(10)}")]
        public async Task<IActionResult> GetPersonWithAccount(string identificacion)
        {
            //ACCION A LA OTRA BASE DE DATOS o ACTIVE DIRECTORY
            var idVerified = await _rGService.ValidateIdentificationNumber(identificacion);
            if (!idVerified)
            {
                return BadRequest(new ErrorResource { ErrorMessage = "Numero de identificacion erroneo"});
            }
            var persona = _personaService.GetPersonWithAccountByCedula(identificacion);
            if (persona == null) 
            {
                ADToDBUser adUser = _activeDirectoryService.FindUserByIdentification(identificacion);
                if(adUser == null) { return NotFound(new ErrorResource { ErrorMessage = "No se encontro persona con esa cedula." }); }
                if(adUser.Persona == null) { return NotFound(new ErrorResource { ErrorMessage = "No se encontro persona con esa cedula." }); }
                
                Debug.WriteLine("Entra al proceso de AD");
                //Se guarda la persona del active directory en la base
                Persona person = adUser.Persona;
                _context.Personas.Add(person);
                _context.SaveChanges();

                //Se guarda la CuentaUsuario en la base
                CuentaUsuario cu = adUser.CuentaUsuario;
                cu.IsActive = true;
                cu.PersonaId = person.Id;
                _context.CuentaUsuarios.Add(cu);
                _context.SaveChanges();
                return Ok(new PersonaConCuentaResource { Success = true , Persona = person, Username=cu.Username});
            }
            return Ok(persona);
            
        }
    }

    
}
