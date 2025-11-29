using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Islemler
{
    public int IslemId { get; set; }

    public string? IslemAdi { get; set; }

    public decimal? Ucret { get; set; }

    public virtual ICollection<Hastaislemleri> Hastaislemleris { get; set; } = new List<Hastaislemleri>();
}
