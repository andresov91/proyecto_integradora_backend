using System.ComponentModel.DataAnnotations;

namespace kairosApp.Resources.Support
{
    public class CrearCuentaResource
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int PersonaId { get; set; }
        public string Password { get; set; }
    }
}
