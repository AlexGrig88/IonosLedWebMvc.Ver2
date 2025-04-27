using System;
using System.Collections.Generic;

namespace IonosLedWebMvc.Ver2.Models;

public partial class ProductsLedLight
{
    public uint Serial { get; set; }

    public uint? ModelId { get; set; }

    public string? Spec { get; set; }

    public uint? BitrixOrder { get; set; }

    public string? Comment { get; set; }

    public uint? LabelPrintUser { get; set; }

    public DateTime? LabelPrintTs { get; set; }

    public uint? AlProfileCutUser { get; set; }

    public DateTime? AlProfileCutTs { get; set; }

    public uint? AlProfileDrillUser { get; set; }

    public DateTime? AlProfileDrillTs { get; set; }

    public uint? LedModuleMountingUser { get; set; }

    public DateTime? LedModuleMountingTs { get; set; }

    public uint? LightSolderingUser { get; set; }

    public DateTime? LightSolderingTs { get; set; }

    public uint? LightAssemblingUser { get; set; }

    public DateTime? LightAssemblingTs { get; set; }

    public uint? LightCheckingPackagingUser { get; set; }

    public DateTime? LightCheckingPackagingTs { get; set; }

    public virtual User? AlProfileCutUserNavigation { get; set; }

    public virtual User? AlProfileDrillUserNavigation { get; set; }

    public virtual User? LabelPrintUserNavigation { get; set; }

    public virtual User? LedModuleMountingUserNavigation { get; set; }

    public virtual User? LightAssemblingUserNavigation { get; set; }

    public virtual User? LightCheckingPackagingUserNavigation { get; set; }

    public virtual User? LightSolderingUserNavigation { get; set; }

    public virtual ModelsLedLight? Model { get; set; }
}
