using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Odalar
{
    public int OdaId { get; set; }

    public string? OdaNumarasi { get; set; }

    public int? Kat { get; set; }

    public int? Kapasite { get; set; }

    public virtual ICollection<Yataklar> Yataklars { get; set; } = new List<Yataklar>();
}
