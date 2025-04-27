using System;
using System.Collections.Generic;

namespace IonosLedWebMvc.Ver2.Models;

public partial class UserEvent
{
    public uint Id { get; set; }

    public uint UserId { get; set; }

    public DateTime Ts { get; set; }

    public string Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
