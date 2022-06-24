using kairosApp.Resources.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Domain.Services
{
    public interface IRGService
    {
        Task<RGDatos> VerifyIdentification(string identificacion, string fecha, string tipo);
        Task<bool> ValidateIdentificationNumber(string identificacion);
    }
}
