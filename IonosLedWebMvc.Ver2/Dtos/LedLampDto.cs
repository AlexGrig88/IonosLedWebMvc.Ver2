using IonosLedWebMvc.Ver2.Models.Entities;
using System.ComponentModel;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class LedLampDto
    {
        [DisplayName("Сер.№")]
        public uint Id { get; set; }

        public uint? ModelId { get; set; }

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

        [DisplayName("Нарезка")]
        public User? CutUser { get; set; }
        [DisplayName("Сверление")]
        public User? DrillUser { get; set; }
        [DisplayName("Печать")]
        public User? LabelPrintUser { get; set; }
        [DisplayName("Монтаж")]
        public User? MountingUser { get; set; }
        [DisplayName("Сборка")]
        public User? AssemblingUser { get; set; }
        [DisplayName("Упаковка")]
        public User? CheckingPackagingUser { get; set; }
        [DisplayName("Пайка")]
        public User? SolderingUser { get; set; }

        [DisplayName("Модель")]
        public LampModel? Model { get; set; }

        public LedLampDto() { }

        public LedLampDto(LedLamp lamp, string employeeName)
        {
            Id = lamp.Id;
            ModelId = lamp.ModelId;
            Spec = lamp?.Spec ?? "";
            BitrixOrder = lamp?.BitrixOrder ?? 0;
            Comment = lamp?.Comment ?? "";
            Model = lamp?.Model ?? new LampModel() { ModelName = "" };

            if (lamp.LabelPrintUser == null || lamp.LabelPrintUser.Name != employeeName) {
                LabelPrintUser = null;
                LabelPrintTs = null;
            } else {
                LabelPrintUser = lamp.LabelPrintUser;
                LabelPrintTs = lamp.LabelPrintTs;
            }

            if (lamp.CutUser == null || lamp.CutUser.Name != employeeName) {
                CutUser = null;
                AlProfileCutTs = null;
            }
            else {
                CutUser = lamp.CutUser;
                AlProfileCutTs = lamp.AlProfileCutTs;
            }

            if (lamp.DrillUser == null || lamp.DrillUser.Name != employeeName) {
                DrillUser = null;
                AlProfileDrillTs = null;
            }
            else {
                DrillUser = lamp.DrillUser;
                AlProfileDrillTs = lamp.AlProfileDrillTs;
            }

            if (lamp.MountingUser == null || lamp.MountingUser.Name != employeeName) {
                MountingUser = null;
                LedModuleMountingTs = null;
            }
            else {
                MountingUser = lamp.MountingUser;
                LedModuleMountingTs = lamp.LedModuleMountingTs;
            }

            if (lamp.AssemblingUser == null || lamp.AssemblingUser.Name != employeeName) {
                AssemblingUser = null;
                LightAssemblingTs = null;
            }
            else {
                AssemblingUser = lamp.AssemblingUser;
                LightAssemblingTs = lamp.LightAssemblingTs;
            }

            if (lamp.SolderingUser == null || lamp.SolderingUser.Name != employeeName) {
                SolderingUser = null;
                LightSolderingTs = null;
            }
            else {
                SolderingUser = lamp.SolderingUser;
                LightSolderingTs = lamp.LightSolderingTs;
            }

            if (lamp.CheckingPackagingUser == null || lamp.CheckingPackagingUser.Name != employeeName) {
                CheckingPackagingUser = null;
                LightCheckingPackagingTs = null;
            }
            else {
                CheckingPackagingUser = lamp.CheckingPackagingUser;
                LightCheckingPackagingTs = lamp.LightCheckingPackagingTs;
            }
        }


        public static LedLampDto FromLedLamp(LedLamp lamp)
        {
            return new LedLampDto
            {
                Id = lamp.Id,
                ModelId = lamp.ModelId,
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
