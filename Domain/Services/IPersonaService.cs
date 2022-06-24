using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using kairosApp.Resources;
using kairosApp.Resources.Support;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Services
{
    public interface IPersonaService
    {
        Task<IEnumerable<Persona>> ListAsync();
        Task<SavePersonaResponse> SaveAsync(Persona persona);
        Task<SavePersonaResponse> UpdateAsync(int id, Persona persona);
        PersonaCuentaResource GetPersonaByCedula(string cedula);
        PersonaConCuentaResource GetPersonWithAccountByCedula(string cedula);
    }
}
