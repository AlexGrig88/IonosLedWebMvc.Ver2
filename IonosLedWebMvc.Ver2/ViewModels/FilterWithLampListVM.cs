using IonosLedWebMvc.Ver2.Dtos;

namespace IonosLedWebMvc.Ver2.ViewModels
{
    public class FilterWithLampListVM
    {
        public LampFilterDto LampFilter { get; set; } = new LampFilterDto();
        public List<LedLampDto> LedLamps { get; set; } = new List<LedLampDto>();

        public LedLampDto LedLampExample { get; set;} = new LedLampDto();
    }
}
