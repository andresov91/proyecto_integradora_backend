using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Services
{
    public interface IGrupoService
    {
        Task<IEnumerable<Grupo>> ListAsync();
        Task<SaveGrupoResponse> SaveAsync(Grupo grupo);
        Task<SaveGrupoResponse> UpdateAsync(int id, Grupo grupo);
    }
}
