using kairosApp.Models;

namespace kairosApp.Domain.Services.Communication
{
    public class SaveSolicitudResponse : BaseResponse
    {
        public Solicitud Solicitud { get; private set; }

        private SaveSolicitudResponse(bool success, string message, Solicitud solicitud) : base(success, message)
        {
            Solicitud = solicitud;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="solicitud">Saved solicitud.</param>
        /// <returns>Response.</returns>
        public SaveSolicitudResponse(Solicitud solicitud) : this(true, string.Empty, solicitud)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveSolicitudResponse(string message) : this(false, message, null)
        { }
    }
}
