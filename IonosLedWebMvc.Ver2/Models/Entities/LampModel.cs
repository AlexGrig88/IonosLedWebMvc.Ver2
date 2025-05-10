using System;
using System.Collections.Generic;

namespace IonosLedWebMvc.Ver2.Models.Entities;

public partial class LampModel
{
    public uint Id { get; set; }
    public string ModelName { get; set; } = null!;
    public byte Sections { get; set; }

    public decimal CutPrice { get; set; }

    public decimal DrillPrice { get; set; }

    public decimal MountPrice { get; set; }

    public decimal SolderPrice { get; set; }

    public decimal AssemblyPrice { get; set; }

    public decimal CheckPrice { get; set; }

    public virtual ICollection<LedLamp> ProductsLedLights { get; set; } = new List<LedLamp>();
}
