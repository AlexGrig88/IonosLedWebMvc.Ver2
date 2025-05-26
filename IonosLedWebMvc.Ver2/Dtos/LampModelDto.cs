using IonosLedWebMvc.Ver2.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class LampModelDto
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Название модели")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        public string ModelName { get; set; } = null!;

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Число секций")]
        public byte Sections { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Нарезка")]
        [RegularExpression(@"^\d{1,9}\.?\d{0,2}$", ErrorMessage = "Некорректный формат денежного числа (разделитель - запятая)")]
        public string CutPrice { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Сверление")]
        [RegularExpression(@"^\d{1,9}\.?\d{0,2}$", ErrorMessage = "Некорректный формат денежного числа (разделитель - запятая)")]
        public string DrillPrice { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Монтаж")]
        [RegularExpression(@"^\d{1,9}\.?\d{0,2}$", ErrorMessage = "Некорректный формат денежного числа (разделитель - запятая)")]
        public string MountPrice { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Пайка")]
        [RegularExpression(@"^\d{1,9}\.?\d{0,2}$", ErrorMessage = "Некорректный формат денежного числа (разделитель - запятая)")]
        public string SolderPrice { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Сборка")]
        [RegularExpression(@"^\d{1,9}\.?\d{0,2}$", ErrorMessage = "Некорректный формат денежного числа (разделитель - запятая)")]
        public string AssemblyPrice { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Упаковка")]
        [RegularExpression(@"^\d{1,9}\.?\d{0,2}$", ErrorMessage = "Некорректный формат денежного числа (разделитель - запятая)")]
        public string CheckPrice { get; set; }

        [DisplayName("Всего изготовлено(шт.)")]
        public int CountReleased { get; set; }

        [DisplayName("Дополнительные сведения")]
        public string Notes {  get; set; } = string.Empty;

        public static LampModelDto FromLampModel(LampModel model, Dictionary<uint, int>? modelsCount = null, string? notes = "")
        {
            if (modelsCount != null && !modelsCount.ContainsKey(model.Id)) {
                modelsCount = null;
            }
            return new LampModelDto()
            {
                Id = model.Id,
                ModelName = model.ModelName,
                Sections = model.Sections,
                CutPrice = model.CutPrice.ToString(),
                DrillPrice = model.DrillPrice.ToString(),
                MountPrice = model.MountPrice.ToString(),
                SolderPrice = model.SolderPrice.ToString(),
                AssemblyPrice = model.AssemblyPrice.ToString(),
                CheckPrice = model.CheckPrice.ToString(),
                CountReleased = modelsCount?[model.Id] ?? 0,
                Notes = notes
            };
        }

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

        public static LampModelDto FromSelfToString (string modelDto)
        {
            string[] arrDataModel = modelDto.Split(";");
            return new LampModelDto()
            {
                Id = uint.Parse(arrDataModel[0]),
                ModelName = arrDataModel[1],
                Sections = byte.Parse(arrDataModel[2]),
                CutPrice = arrDataModel[3],
                DrillPrice = arrDataModel[4],
                MountPrice = arrDataModel[5],
                SolderPrice = arrDataModel[6],
                AssemblyPrice = arrDataModel[7],
                CheckPrice = arrDataModel[8],
            };
        }

        public override string ToString() => $"{Id};{ModelName};{Sections};{CutPrice};{DrillPrice};{MountPrice};{SolderPrice};{AssemblyPrice};{CheckPrice}";
    }
}
