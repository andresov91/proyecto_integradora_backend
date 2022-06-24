using kairosApp.Models;

namespace kairosApp.Domain.Services.Communication
{
    public class SaveUsuarioGrupoResponse : BaseResponse
    {
        public UsuarioGrupo UsuarioGrupo { get; private set; }

        private SaveUsuarioGrupoResponse(bool success, string message, UsuarioGrupo usuarioGrupo) : base(success, message)
        {
            UsuarioGrupo = usuarioGrupo;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="cuentaUsuario">Saved Persona.</param>
        /// <returns>Response.</returns>
        public SaveUsuarioGrupoResponse(UsuarioGrupo usuarioGrupo) : this(true, string.Empty, usuarioGrupo)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveUsuarioGrupoResponse(string message) : this(false, message, null)
        { }
    }
}
