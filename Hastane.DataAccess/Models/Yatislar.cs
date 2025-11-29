using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Yatislar
{
    public int YatisId { get; set; }

    public string? HastaTc { get; set; }

    public int? YatakId { get; set; }

    public DateTime? GirisTarihi { get; set; }

    public DateTime? CikisTarihi { get; set; }

    public virtual Yataklar? Yatak { get; set; }
}
