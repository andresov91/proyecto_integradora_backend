using kairosApp.Models;

namespace kairosApp.Resources
{
    public class SolicitudInfoResource
    {
        public int Id { get; set; }
        public string Estado {get; set; }
        public InfoSolicitud InfoSolicitud {get; set; }
    }
}
