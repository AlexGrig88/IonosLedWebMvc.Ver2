using IonosLedWebMvc.Ver2.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class LampModelDto
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Название модели")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 20 символов")]
        public string ModelName { get; set; } = null!;

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Число секций")]
        public byte Sections { get; set; }

        [DisplayName("Нарезка")]
        [RegularExpression(@"^\d{1,9}\,?\d{0,2}$")]
        public string CutPrice { get; set; }

        [DisplayName("Сверление")]
        [RegularExpression(@"^\d{1,9}\,?\d{0,2}$")]
        public string DrillPrice { get; set; }
        

        [DisplayName("Монтаж")]
        [RegularExpression(@"^\d{1,9}\,?\d{0,2}$")]
        public string MountPrice { get; set; }

        [DisplayName("Пайка")]
        [RegularExpression(@"^\d{1,9}\,?\d{0,2}$")]
        public string SolderPrice { get; set; }

        [DisplayName("Сборка")]
        [RegularExpression(@"^\d{1,9}\,?\d{0,2}$")]
        public string AssemblyPrice { get; set; }

        [DisplayName("Упаковка")]
        [RegularExpression(@"^\d{1,9}\,?\d{0,2}$")]
        public string CheckPrice { get; set; }

        public static LampModelDto FromLampModel(LampModel model) =>
            new LampModelDto()
            {
                Id = model.Id,
                ModelName = model.ModelName,
                Sections = model.Sections,
                CutPrice = model.CutPrice.ToString(),
                DrillPrice = model.DrillPrice.ToString(),
                MountPrice = model.MountPrice.ToString(),
                SolderPrice = model.SolderPrice.ToString(),
                AssemblyPrice = model.AssemblyPrice.ToString(),
                CheckPrice = model.CheckPrice.ToString()
            };

        public static LampModel ToLampModel(LampModelDto modelDto) =>
            new LampModel()
            {
                ModelName = modelDto.ModelName,
                Sections = modelDto.Sections,
                CutPrice = Convert.ToDecimal(modelDto.CutPrice),
                DrillPrice = Convert.ToDecimal(modelDto.DrillPrice),
                MountPrice = Convert.ToDecimal(modelDto.MountPrice),
                SolderPrice = Convert.ToDecimal(modelDto.SolderPrice),
                AssemblyPrice = Convert.ToDecimal(modelDto.AssemblyPrice),
                CheckPrice = Convert.ToDecimal(modelDto.CheckPrice)
            };
    }
}
