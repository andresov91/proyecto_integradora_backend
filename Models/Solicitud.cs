using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string InfoSolicitud { get; set; }
        
        public SolicitudPersona SolicitudPersona { get; set; }
    }
}
