using IonosLedWebMvc.Ver2.Models;
using System.ComponentModel;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class LedLampDto
    {
        public uint Id { get; set; }

        [DisplayName("Спека")]
        public string? Spec { get; set; }

        [DisplayName("BitrixНомер")]
        public uint? BitrixOrder { get; set; }

        [DisplayName("Коммент")]
        public string? Comment { get; set; }

        [DisplayName("ПечатьЭтикеткиTs")]
        public DateTime? LabelPrintTs { get; set; }
        [DisplayName("НарезкаTs")]
        public DateTime? AlProfileCutTs { get; set; }
        [DisplayName("СверлениеTs")]
        public DateTime? AlProfileDrillTs { get; set; }
        [DisplayName("МонтажTs")]
        public DateTime? LedModuleMountingTs { get; set; }
        [DisplayName("ПайкаTs")]
        public DateTime? LightSolderingTs { get; set; }
        [DisplayName("СборкаTs")]
        public DateTime? LightAssemblingTs { get; set; }
        [DisplayName("УпаковкаTs")]
        public DateTime? LightCheckingPackagingTs { get; set; }

        [DisplayName("НарезкаUser")]
        public User? CutUser { get; set; }
        [DisplayName("СверлениеUser")]
        public User? DrillUser { get; set; }
        [DisplayName("ПечатьUser")]
        public User? LabelPrintUser { get; set; }
        [DisplayName("МонтажUser")]
        public User? MountingUser { get; set; }
        [DisplayName("СборкаUser")]
        public User? AssemblingUser { get; set; }
        [DisplayName("УпаковкаUser")]
        public User? CheckingPackagingUser { get; set; }
        [DisplayName("ПайкаUser")]
        public User? SolderingUser { get; set; }

        [DisplayName("Модель")]
        public LampModel? Model { get; set; }

        public static LedLampDto FromLedLamp(LedLamp lamp)
        {
            return new LedLampDto
            {
                Id = lamp.Id,
                Spec = lamp?.Spec ?? "",
                BitrixOrder = lamp?.BitrixOrder ?? 0,
                Comment = lamp?.Comment ?? "",
                LabelPrintTs = lamp?.LabelPrintTs ?? new DateTime(),
                AlProfileCutTs = lamp?.AlProfileCutTs ?? new DateTime(),
                AlProfileDrillTs = lamp?.AlProfileDrillTs ?? new DateTime(),
                LedModuleMountingTs = lamp?.LedModuleMountingTs ?? new DateTime(),
                LightAssemblingTs = lamp?.LightAssemblingTs ?? new DateTime(),
                LightCheckingPackagingTs = lamp?.LightCheckingPackagingTs ?? new DateTime(),
                LightSolderingTs = lamp?.LightSolderingTs ?? new DateTime(),

                CutUser = lamp?.CutUser ?? new User() { Name = "" },
                DrillUser = lamp?.DrillUser ?? new User() { Name = "" },
                LabelPrintUser = lamp?.LabelPrintUser ?? new User() { Name = "" },
                MountingUser = lamp?.MountingUser ?? new User() { Name = "" },
                AssemblingUser = lamp?.AssemblingUser ?? new User() { Name = "" },
                CheckingPackagingUser = lamp?.CheckingPackagingUser ?? new User() { Name = "" },
                SolderingUser = lamp?.SolderingUser ?? new User() { Name = "" },
                Model = lamp?.Model ??  new LampModel() { ModelName = "" }
            };
        }
    }
}
