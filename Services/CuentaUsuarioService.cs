using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Models;
using kairosApp.Domain.Services.Communication;
using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Models.Support;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

namespace kairosApp.Services
{
    public class CuentaUsuarioService : ICuentaUsuarioService
    {
        private readonly ICuentaUsuarioRepository _cuentaUsuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public CuentaUsuarioService(ICuentaUsuarioRepository cuentaUsuarioRepository, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _cuentaUsuarioRepository = cuentaUsuarioRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public string CreateNewPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(5, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(3, false));
            return builder.ToString();
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public async Task<IEnumerable<CuentaUsuario>> ListAsync()
        {
            return await _cuentaUsuarioRepository.ListAsync();
        }

        public async Task<SaveCuentaUsuarioResponse> SaveAsync(CuentaUsuario cuenta)
        {
            try
            {
                await _cuentaUsuarioRepository.AddAsync(cuenta);
                await _unitOfWork.CompleteAsync();

                return new SaveCuentaUsuarioResponse(cuenta);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveCuentaUsuarioResponse($"Un error ocurrio mientras se guardaba la cuenta: {ex.Message}");
            }
        }

        public async Task<SaveCuentaUsuarioResponse> UpdateAsync(int id, CuentaUsuario cuentaUsuario)
        {
            var existingCuenta = await _cuentaUsuarioRepository.FindByIdAsync(id);

            if (existingCuenta == null)
                return new SaveCuentaUsuarioResponse("Cuenta no Encontrada.");

            /*existingCuenta.Alias = cuentaUsuario.Alias;
            existingCuenta.Username = cuentaUsuario.Username;*/
            existingCuenta.IsActive = cuentaUsuario.IsActive;

            try
            {
                _cuentaUsuarioRepository.Update(existingCuenta);
                await _unitOfWork.CompleteAsync();

                return new SaveCuentaUsuarioResponse(existingCuenta);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveCuentaUsuarioResponse($"Un error ocurrio mientras se actualizaba a la persona: {ex.Message}");
            }
        }

        public bool VerifyAlias(string alias)
        {
            var aliasExistente = _context.CuentaUsuarios.Where(p => p.Alias == alias).Any();
            return aliasExistente;
        }

        public bool VerifyEmail(PersonResetPasswordCredentials credentials)
        {
            var usuario = _context.CuentaUsuarios.FirstOrDefault(p => p.PersonaId == credentials.PersonaId);
            if (usuario == null) { return false; }
            var persona = _context.Personas.Find(usuario.PersonaId);
            Debug.WriteLine("Devuelve en el 2do if: "+persona.CorreoAlterno);
            if (usuario != null && persona != null && persona.CorreoAlterno.Equals(credentials.Correo))
            {
                Debug.WriteLine("Devuelve en el 2do if: ");
                return true;
            }
            Debug.WriteLine("Devuelve al final");
            return false;
        }

        public bool VerifyUsername(string username)
        {
            var usernameExistente = _context.CuentaUsuarios.Where(p => p.Username == username).Any();
            return usernameExistente;
        }
    }
}
