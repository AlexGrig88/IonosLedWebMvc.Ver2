using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.Models.Entities
{
    public class UserAuth
    {
        public int Id { get; set; }

        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
