using System;
using System.Collections.Generic;

namespace IonosLedWebMvc.Ver2.Models.Entities;

public partial class User
{
    public uint Id { get; set; }

    public string Name { get; set; } = null!;

    public ushort Pin { get; set; }

    public uint RoleId { get; set; }

    public virtual ICollection<LedLamp> ProductsCut { get; set; } = new List<LedLamp>();

    public virtual ICollection<LedLamp> ProductsDrill { get; set; } = new List<LedLamp>();

    public virtual ICollection<LedLamp> ProductsLabelPrint { get; set; } = new List<LedLamp>();

    public virtual ICollection<LedLamp> ProductsMounting { get; set; } = new List<LedLamp>();

    public virtual ICollection<LedLamp> ProductsAssembling { get; set; } = new List<LedLamp>();

    public virtual ICollection<LedLamp> ProductsPackaging { get; set; } = new List<LedLamp>();

    public virtual ICollection<LedLamp> ProductsSoldering { get; set; } = new List<LedLamp>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

    public override string ToString() => $"UserName = {Name}; Pin = {Pin}";
}
