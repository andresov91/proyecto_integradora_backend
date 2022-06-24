using System;

namespace kairosApp.Models
{
    public class UserTokens
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public TimeSpan Validaty { get; set; }
        public string RefreshToken { get; set; }
        public int Id { get; set; }
        public Persona Persona { get; set; }
        public DateTime ExpiredTime{ get; set; }
    }
}
