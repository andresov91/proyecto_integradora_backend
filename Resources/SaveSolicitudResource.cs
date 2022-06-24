using System;
using System.ComponentModel.DataAnnotations;

namespace kairosApp.Resources
{
    public class SaveSolicitudResource
    {
        public int Id { get; set; }
        [Required]
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string InfoSolicitud { get; set; }
    }
}
