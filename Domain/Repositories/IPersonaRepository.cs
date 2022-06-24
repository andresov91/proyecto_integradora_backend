using kairosApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Repositories
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> ListAsync();
        Task AddAsync(Persona persona);
        Task<Persona> FindByIdAsync(int id);
        void Update(Persona persona);
        Task<Persona> FindByCedula(string cedula);
    }
}
