using IonosLedWebMvc.Ver2.Models.Entities;
using System.ComponentModel;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class UserEventDto
    {
        public uint Id { get; set; }
        public uint UserId { get; set; }
        [DisplayName("Метка времени")]
        public DateTime Ts { get; set; }
        [DisplayName("Системное событие")]
        public string? Event { get; set; }

        public static UserEventDto FromUserEvent(UserEvent uEvent)
        {
            return new UserEventDto
            {
                Id = uEvent.Id,
                UserId = uEvent.UserId,
                Ts = uEvent.Ts,
                Event = uEvent.Event
            };
        }
    }
}
