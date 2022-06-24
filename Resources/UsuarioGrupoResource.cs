namespace kairosApp.Resources
{
    public class UsuarioGrupoResource
    {
        public int Id { get; set; }
        //public int CuentaUsuarioId { get; set; }
        public GrupoResource Grupo { get; set; }
    }
}
