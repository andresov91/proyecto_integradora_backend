using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Repositories
{
    public interface IUsuarioGrupoRepository
    {
        Task<IEnumerable<UsuarioGrupo>> ListAsync();
        Task AddAsync(UsuarioGrupo usuarioGrupo);
        Task<UsuarioGrupo> FindByIdAsync(int id);
        void Update(UsuarioGrupo usuarioGrupo);
    }
}
