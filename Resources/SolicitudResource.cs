using System;

namespace kairosApp.Resources
{
    public class SolicitudResource
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string InfoSolicitud { get; set; }   
    }
}
