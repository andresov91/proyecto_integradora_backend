using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public IList<UsuarioGrupo> UsuarioGrupos { get; set; } = new List<UsuarioGrupo>();

    }
}
