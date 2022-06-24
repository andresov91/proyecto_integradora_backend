using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    
    public class Persona
    {
        public int Id { get; set; }
        public string Identificacion { get; set; }  
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Unidad { get; set; }
        public string CorreoAlterno { get; set; }
        public string Rol { get; set; }
        public CuentaUsuario CuentaUsuario;
        public IList<SolicitudPersona> SolicitudPersonas  { get; set; } = new List<SolicitudPersona>();


    }
}
