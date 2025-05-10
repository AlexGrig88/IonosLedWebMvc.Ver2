using IonosLedWebMvc.Ver2.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class RoleDto
    {
        public uint Id { get; set; }

        [DisplayName("Должность")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string RoleName { get; set; } = null!;

        [DisplayName("Настройка")]
        public bool PermissionSettings { get; set; }
        [DisplayName("Нарезка")]
        public bool PermissionAlProfileCut { get; set; }
        [DisplayName("Сверление")]
        public bool PermissionAlProfileDrill { get; set; }
        [DisplayName("Монтаж")]
        public bool PermissionLedModuleMounting { get; set; }
        [DisplayName("Пайка")]
        public bool PermissionLightSoldering { get; set; }
        [DisplayName("Сборка")]
        public bool PermissionLightAssembling { get; set; }
        [DisplayName("Упаковка")]
        public bool PermissionLightCheckingPackaging { get; set; }
        [DisplayName("Главный производства")]
        public bool PermissionChiefLightProduction { get; set; }

        public static RoleDto FromRole(Role role) =>
            new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName,
                PermissionSettings = role.PermissionSettings,
                PermissionAlProfileCut = role.PermissionAlProfileCut,
                PermissionAlProfileDrill = role.PermissionAlProfileDrill,
                PermissionLedModuleMounting = role.PermissionLedModuleMounting,
                PermissionLightSoldering = role.PermissionLightSoldering,
                PermissionLightAssembling = role.PermissionLightAssembling,
                PermissionLightCheckingPackaging = role.PermissionLightCheckingPackaging,
                PermissionChiefLightProduction = role.PermissionChiefLightProduction
            };

        public static Role ToRole(RoleDto roleDto) =>
            new Role()
            {
                RoleName = roleDto.RoleName,
                PermissionSettings = roleDto.PermissionSettings,
                PermissionAlProfileCut = roleDto.PermissionAlProfileCut,
                PermissionAlProfileDrill = roleDto.PermissionAlProfileDrill,
                PermissionLedModuleMounting = roleDto.PermissionLedModuleMounting,
                PermissionLightSoldering = roleDto.PermissionLightSoldering,
                PermissionLightAssembling = roleDto.PermissionLightAssembling,
                PermissionLightCheckingPackaging = roleDto.PermissionLightCheckingPackaging,
                PermissionChiefLightProduction = roleDto.PermissionChiefLightProduction
            };
    }
}
