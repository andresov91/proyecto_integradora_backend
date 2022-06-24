using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Services
{
    public interface IUsuarioGrupoService
    {
        Task<IEnumerable<UsuarioGrupo>> ListAsync();
        Task<SaveUsuarioGrupoResponse> SaveAsync(UsuarioGrupo usuarioGrupo);
        Task<SaveUsuarioGrupoResponse> UpdateAsync(int id, UsuarioGrupo usuarioGrupo);

        bool UpdateUsuarioGrupos(int usuarioId, List<int> gruposIds);
    }
}
