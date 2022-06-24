using System.ComponentModel.DataAnnotations;

namespace kairosApp.Resources
{
    public class SaveUsuarioGrupoResource
    {
        public int Id { get; set; }
        [Required]
        public int GrupoId { get; set; }
        [Required]
        public int CuentaUsuarioId { get; set; }
    }
}
