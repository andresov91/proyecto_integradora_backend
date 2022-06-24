using kairosApp.Models;

namespace kairosApp.Domain.Services.Communication
{
    public class SaveGrupoResponse : BaseResponse
    {
        public Grupo Grupo { get; private set; }

        private SaveGrupoResponse(bool success, string message, Grupo grupo) : base(success, message)
        {
            Grupo = grupo;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="grupo">Saved Grupo.</param>
        /// <returns>Response.</returns>
        public SaveGrupoResponse(Grupo grupo) : this(true, string.Empty, grupo)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveGrupoResponse(string message) : this(false, message, null)
        { }
    }
}
