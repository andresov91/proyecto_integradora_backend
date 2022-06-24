using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using kairosApp.Resources;
using kairosApp.Resources.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public PersonaService(IPersonaRepository personaRepository, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _personaRepository = personaRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public PersonaCuentaResource GetPersonaByCedula(string cedula)
        {
            try
            {
                var persona = _context.Personas.Where(p => p.Identificacion == cedula).Single();
                var cuenta = _context.CuentaUsuarios.Where(p => p.PersonaId == persona.Id).ToList();
                if (cuenta.Any())
                {
                    return new PersonaCuentaResource { Persona = null};
                }
                var usuarios = GetUsers(persona.Nombres, persona.Apellidos);
                var personaCuenta = new PersonaCuentaResource { Persona = persona, Usuarios = usuarios };
                return personaCuenta;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("En el catch: " + ex.Message);
                return null;
            }
        }

        public PersonaConCuentaResource GetPersonWithAccountByCedula(string cedula)
        {
            try
            {
                var persona = _context.Personas.Where(p => p.Identificacion == cedula).Single();
                var cuenta = _context.CuentaUsuarios.Where(p => p.PersonaId == persona.Id).ToList();
                if (cuenta.Any())
                {
                    
                    return new PersonaConCuentaResource { Success = true, Persona = persona, Username =cuenta.First().Username};
                }
                return new PersonaConCuentaResource { Success = false, Persona = null };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private IList<string> GetUsers(string nombres, string apellidos)
        {
            var contador = 0;
            var users = new List<string>();
            var nombresLista = nombres.Trim().Split(' ');
            var apellidosLista = apellidos.Trim().Split(' ');
            Debug.WriteLine(apellidosLista.ToString());
            while(contador < 3)
            {
                foreach (var n in nombresLista)
                {
                    foreach (var a in apellidosLista)
                    {
                        var nombreUsuario = "";
                        Debug.WriteLine("apellido " + a);
                        Debug.WriteLine(n.Length+" "+a.Length);
                        if (n.Length < 4 && a.Length < 4)
                        {
                            continue;
                        }
                        if(n.Length >=4 && (a.Length < 4 && a.Length != 0))
                        {
                            nombreUsuario = n.Substring(0, 4).ToLower() + a.Substring(0, a.Length).ToLower();
                        }else if (a.Length >= 4 && (n.Length < 4 && n.Length != 0))
                        {
                            nombreUsuario = n.Substring(0, n.Length).ToLower() + a.Substring(0, 4).ToLower();
                        }
                        else
                        {
                            nombreUsuario = n.Substring(0, 4).ToLower() + a.Substring(0, 4).ToLower();
                        }
                        
                        var usuarios = _context.CuentaUsuarios.Where(p => p.Username == nombreUsuario).ToList();
                        if (!usuarios.Any())
                        {
                            users.Add(nombreUsuario);
                            contador++;
                            if(contador == 3)
                            {
                                return users;
                            }
                        }
                    }
                }
                
            }
            return users;
            
        }
        public async Task<IEnumerable<Persona>> ListAsync()
        {
            return await _personaRepository.ListAsync();
        }

        public async Task<SavePersonaResponse> SaveAsync(Persona persona)
        {
            try
            {
                await _personaRepository.AddAsync(persona);
                await _unitOfWork.CompleteAsync();

                return new SavePersonaResponse(persona);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SavePersonaResponse($"Un error ocurrio mientras se guardaba la persona: {ex.Message}");
            }
        }

        public async Task<SavePersonaResponse> UpdateAsync(int id, Persona persona)
        {
            var existingPersona = await _personaRepository.FindByIdAsync(id);

            if (existingPersona == null)
                return new SavePersonaResponse("Persona no Encontrada.");

            existingPersona.Nombres = persona.Nombres;
            existingPersona.Apellidos = persona.Apellidos;
            existingPersona.Telefono = persona.Telefono;
            existingPersona.CorreoAlterno = persona.CorreoAlterno;
            /*existingPersona.Identificacion = persona.Identificacion;
            existingPersona.Rol = persona.Rol;*/

            try
            {
                _personaRepository.Update(existingPersona);
                await _unitOfWork.CompleteAsync();

                return new SavePersonaResponse(existingPersona);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SavePersonaResponse($"Un error ocurrio mientras se actualizaba a la persona: {ex.Message}");
            }
        }
    }

    
}
