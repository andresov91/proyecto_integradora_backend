using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    public class CuentaUsuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public int PersonaId { get; set; }
        public Persona Persona { get; set; }
        public IList<UsuarioGrupo> UsuarioGrupo { get; set; } = new List<UsuarioGrupo>();

    }
}
