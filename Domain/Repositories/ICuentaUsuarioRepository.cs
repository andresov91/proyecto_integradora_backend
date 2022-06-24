using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Repositories
{
    public interface ICuentaUsuarioRepository
    {
        Task<IEnumerable<CuentaUsuario>> ListAsync();
        Task AddAsync(CuentaUsuario cuentaUsuario);
        Task<CuentaUsuario> FindByIdAsync(int id);
        void Update(CuentaUsuario perscuentaUsuarioona);
    }
}
