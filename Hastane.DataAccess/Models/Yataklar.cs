using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Yataklar
{
    public int YatakId { get; set; }

    public int? OdaId { get; set; }

    public string? YatakNo { get; set; }

    public bool? DoluMu { get; set; }

    public virtual Odalar? Oda { get; set; }

    public virtual ICollection<Yatislar> Yatislars { get; set; } = new List<Yatislar>();
}
