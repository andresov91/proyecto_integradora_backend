using kairosApp.Models;
using System.Collections.Generic;

namespace kairosApp.Resources
{
    public class CuentaUsuarioResource
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public string Alias { get; set; }
        public PersonaResource Persona { get; set; }
        public IList<UsuarioGrupoResource> UsuarioGrupo { get; set; }


    }
}
