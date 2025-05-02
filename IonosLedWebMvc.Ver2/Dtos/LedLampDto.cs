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
        public virtual string? CutUser { get; set; }
        [DisplayName("СверлениеUser")]
        public virtual string? DrillUser { get; set; }
        [DisplayName("ПечатьUser")]
        public virtual string? LabelPrintUser { get; set; }
        [DisplayName("МонтажUser")]
        public virtual string? MountingUser { get; set; }
        [DisplayName("СборкаUser")]
        public virtual string? AssemblingUser { get; set; }
        [DisplayName("УпаковкаUser")]
        public virtual string? CheckingPackagingUser { get; set; }
        [DisplayName("ПайкаUser")]
        public virtual string? SolderingUser { get; set; }
        [DisplayName("Модель")]
        public virtual string? ModelName { get; set; }

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

                CutUser = lamp?.CutUser?.Name ?? "",
                DrillUser = lamp?.DrillUser?.Name ?? "",
                LabelPrintUser = lamp?.LabelPrintUser?.Name ?? "",
                MountingUser = lamp?.MountingUser?.Name ?? "",
                AssemblingUser = lamp?.AssemblingUser?.Name ?? "",
                CheckingPackagingUser = lamp?.CheckingPackagingUser?.Name ?? "",
                SolderingUser = lamp?.SolderingUser?.Name ?? "",
                ModelName = lamp?.Model?.ModelName ?? ""
            };
        }
    }
}
