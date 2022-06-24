using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Repositories
{
    public interface IGrupoRepository
    {
        Task<IEnumerable<Grupo>> ListAsync();
        Task AddAsync(Grupo grupo);
        Task<Grupo> FindByIdAsync(int id);
        void Update(Grupo grupo);
    }
}
