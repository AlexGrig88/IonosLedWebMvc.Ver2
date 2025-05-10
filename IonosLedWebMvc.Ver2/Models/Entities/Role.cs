using System;
using System.Collections.Generic;

namespace IonosLedWebMvc.Ver2.Models.Entities;

public partial class Role
{
    public uint Id { get; set; }

    public string RoleName { get; set; } = null!;

    public bool PermissionSettings { get; set; }

    public bool PermissionAlProfileCut { get; set; }

    public bool PermissionAlProfileDrill { get; set; }

    public bool PermissionLedModuleMounting { get; set; }

    public bool PermissionLightSoldering { get; set; }

    public bool PermissionLightAssembling { get; set; }

    public bool PermissionLightCheckingPackaging { get; set; }

    public bool PermissionChiefLightProduction { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
