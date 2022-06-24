using kairosApp.Models;

namespace kairosApp.Resources.Support
{
    public class PersonaConCuentaResource
    {
        public bool Success { get; set; }
        public Persona Persona { get; set; }
        public string Username { get; set; }
    }
}
