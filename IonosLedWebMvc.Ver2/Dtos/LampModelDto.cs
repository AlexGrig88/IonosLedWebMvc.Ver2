using IonosLedWebMvc.Ver2.Models;
using System.ComponentModel;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class LampModelDto
    {
        public uint Id { get; set; }
        [DisplayName("Имя модели")]
        public string ModelName { get; set; } = null!;
        [DisplayName("Число секций")]
        public byte Sections { get; set; }

        [DisplayName("Нарезка")]
        public decimal CutPrice { get; set; }
        [DisplayName("Сверление")]
        public decimal DrillPrice { get; set; }
        [DisplayName("Монтаж")]
        public decimal MountPrice { get; set; }
        [DisplayName("Пайка")]
        public decimal SolderPrice { get; set; }
        [DisplayName("Сборка")]
        public decimal AssemblyPrice { get; set; }
        [DisplayName("Упаковка")]
        public decimal CheckPrice { get; set; }

        public static LampModelDto FromLampModel(LampModel model) =>
            new LampModelDto()
            {
                Id = model.Id,
                ModelName = model.ModelName,
                Sections = model.Sections,
                CutPrice = model.CutPrice,
                DrillPrice = model.DrillPrice,
                MountPrice = model.MountPrice,
                SolderPrice = model.SolderPrice,
                AssemblyPrice = model.AssemblyPrice,
                CheckPrice = model.CheckPrice
            };

        public static LampModel ToLampModel(LampModelDto modelDto) =>
            new LampModel()
            {
                ModelName = modelDto.ModelName,
                Sections = modelDto.Sections,
                CutPrice = modelDto.CutPrice,
                DrillPrice = modelDto.DrillPrice,
                MountPrice = modelDto.MountPrice,
                SolderPrice = modelDto.SolderPrice,
                AssemblyPrice = modelDto.AssemblyPrice,
                CheckPrice = modelDto.CheckPrice
            };
    }
}
