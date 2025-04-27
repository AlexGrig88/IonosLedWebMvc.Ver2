using System;
using System.Collections.Generic;

namespace IonosLedWebMvc.Ver2.Models;

public partial class LedLamp
{
    public uint Id { get; set; }
    public uint? ModelId { get; set; }
    public string? Spec { get; set; }
    public uint? BitrixOrder { get; set; }
    public string? Comment { get; set; }

    public uint? LabelPrintUserId { get; set; }
    public DateTime? LabelPrintTs { get; set; }

    public uint? CutUserId { get; set; }
    public DateTime? AlProfileCutTs { get; set; }


    public uint? DrillUserId { get; set; }
    public DateTime? AlProfileDrillTs { get; set; }


    public uint? MountingUserId { get; set; }
    public DateTime? LedModuleMountingTs { get; set; }

    public uint? SolderingUserId { get; set; }
    public DateTime? LightSolderingTs { get; set; }

    public uint? AssemblingUserId { get; set; }
    public DateTime? LightAssemblingTs { get; set; }

    public uint? CheckingPackagingUserId { get; set; }
    public DateTime? LightCheckingPackagingTs { get; set; }


    public virtual User? CutUser { get; set; }
    public virtual User? DrillUser { get; set; }
    public virtual User? LabelPrintUser { get; set; }
    public virtual User? MountingUser { get; set; }
    public virtual User? AssemblingUser { get; set; }
    public virtual User? CheckingPackagingUser { get; set; }
    public virtual User? SolderingUser { get; set; }

    public virtual LampModel? Model { get; set; }
}
