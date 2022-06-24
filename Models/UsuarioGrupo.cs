using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    public class UsuarioGrupo
    {
        public int Id { get; set; } 
        public int CuentaUsuarioId { get; set; }
        public CuentaUsuario CuentaUsuario { get; set; }
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
        
    }
}
