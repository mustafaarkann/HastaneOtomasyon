using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Muayeneler
{
    public int MuayeneId { get; set; }

    public int? RandevuId { get; set; }

    public string? Teshis { get; set; }

    public string? DoktorNotu { get; set; }

    public string? ReceteNo { get; set; }

    public virtual ICollection<Hastaislemleri> Hastaislemleris { get; set; } = new List<Hastaislemleri>();

    public virtual Randevular? Randevu { get; set; }
}
