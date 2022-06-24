using System.ComponentModel.DataAnnotations;

namespace kairosApp.Resources
{
    public class SaveGrupoResource
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
    }
}
