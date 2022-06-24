using kairosApp.Models;

namespace kairosApp.Domain.Services.Communication
{
    public class SaveCuentaUsuarioResponse : BaseResponse
    {
        public CuentaUsuario CuentaUsuario { get; private set; }

        private SaveCuentaUsuarioResponse(bool success, string message, CuentaUsuario cuentaUsuario) : base(success, message)
        {
            CuentaUsuario = cuentaUsuario;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="cuentaUsuario">Saved Persona.</param>
        /// <returns>Response.</returns>
        public SaveCuentaUsuarioResponse(CuentaUsuario cuentaUsuario) : this(true, string.Empty, cuentaUsuario)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveCuentaUsuarioResponse(string message) : this(false, message, null)
        { }
    }
}
