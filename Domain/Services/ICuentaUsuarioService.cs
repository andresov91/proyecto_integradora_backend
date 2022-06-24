using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using kairosApp.Models.Support;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Services
{
    public interface ICuentaUsuarioService
    {
        Task<IEnumerable<CuentaUsuario>> ListAsync();
        Task<SaveCuentaUsuarioResponse> SaveAsync(CuentaUsuario cuentaUsuario);
        Task<SaveCuentaUsuarioResponse> UpdateAsync(int id, CuentaUsuario cuentaUsuario);
        bool VerifyAlias(string alias);
        bool VerifyUsername(string username);
        bool VerifyEmail(PersonResetPasswordCredentials credentials);
        string CreateNewPassword();
    }
}
