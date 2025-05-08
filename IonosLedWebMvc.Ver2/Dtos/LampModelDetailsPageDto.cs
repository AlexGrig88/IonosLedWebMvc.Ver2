using IonosLedWebMvc.Ver2.Models;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class LampModelDetailsPageDto
    {
        public LampModelDto LampModel { get; set; }
        // public List<LedLampDto> Lamps { get; set; }
        public PaginatedList<LedLamp> LampsPaginated { get; set; }

        public LampModelDetailsPageDto(LampModelDto lampModel, PaginatedList<LedLamp> lamps)
        {
            LampModel = lampModel;
            LampsPaginated = lamps;
        }
    }
}
