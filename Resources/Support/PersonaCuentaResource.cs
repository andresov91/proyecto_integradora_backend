using kairosApp.Models;
using System.Collections.Generic;

namespace kairosApp.Resources
{
    public class PersonaCuentaResource
    {
        public Persona Persona { get; set; }
        public IList<string> Usuarios { get; set; }
    }
}
