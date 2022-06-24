using kairosApp.Models;

namespace kairosApp.Domain.Services.Communication
{
    public class SavePersonaResponse : BaseResponse
    {
        public Persona Persona { get; private set; }

        private SavePersonaResponse(bool success, string message, Persona persona) : base(success, message)
        {
            Persona = persona;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="persona">Saved Persona.</param>
        /// <returns>Response.</returns>
        public SavePersonaResponse(Persona persona) : this(true, string.Empty, persona)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SavePersonaResponse(string message) : this(false, message, null)
        { }
    }
}
