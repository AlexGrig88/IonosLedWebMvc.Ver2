using IonosLedWebMvc.Ver2.Models;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class UserEventDto
    {
        public uint Id { get; set; }
        public DateTime Ts { get; set; }
        public string? Event { get; set; }
        public User? User { get; set; }
    }
}
