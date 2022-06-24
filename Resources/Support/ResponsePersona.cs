using kairosApp.Models;

namespace kairosApp.Resources.Support
{
    public class ResponsePersona
    {
        public bool Success { get; set; }
        public PersonaCuentaResource Persona { get; set; }
    }
}
