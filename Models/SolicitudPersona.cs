namespace kairosApp.Models
{
    public class SolicitudPersona
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }

        public Solicitud Solicitud { get; set; }
        public int PersonaId { get; set; }
        public Persona Persona { get; set; }
    }
}
