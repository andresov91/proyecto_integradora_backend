using System.ComponentModel.DataAnnotations;
namespace kairosApp.Resources
{
    public class SavePersonaResource
    {
        
        public int Id { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Nombres { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Apellidos { get; set; }
        
        public string Identificacion { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Telefono { get; set; }
        
        public string Rol { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string CorreoAlterno { get; set; }
        
        public string Unidad { get; set; }
    }
}
