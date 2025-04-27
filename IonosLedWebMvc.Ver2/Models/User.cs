using System;
using System.Collections.Generic;

namespace IonosLedWebMvc.Ver2.Models;

public partial class User
{
    public uint Id { get; set; }

    public string Name { get; set; } = null!;

    public ushort Pin { get; set; }

    public uint Role { get; set; }

    public virtual ICollection<ProductsLedLight> ProductsLedLightAlProfileCutUserNavigations { get; set; } = new List<ProductsLedLight>();

    public virtual ICollection<ProductsLedLight> ProductsLedLightAlProfileDrillUserNavigations { get; set; } = new List<ProductsLedLight>();

    public virtual ICollection<ProductsLedLight> ProductsLedLightLabelPrintUserNavigations { get; set; } = new List<ProductsLedLight>();

    public virtual ICollection<ProductsLedLight> ProductsLedLightLedModuleMountingUserNavigations { get; set; } = new List<ProductsLedLight>();

    public virtual ICollection<ProductsLedLight> ProductsLedLightLightAssemblingUserNavigations { get; set; } = new List<ProductsLedLight>();

    public virtual ICollection<ProductsLedLight> ProductsLedLightLightCheckingPackagingUserNavigations { get; set; } = new List<ProductsLedLight>();

    public virtual ICollection<ProductsLedLight> ProductsLedLightLightSolderingUserNavigations { get; set; } = new List<ProductsLedLight>();

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();
}
