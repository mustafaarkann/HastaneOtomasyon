using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Randevular
{
    public int RandevuId { get; set; }

    public string? HastaTc { get; set; }

    public int? DoktorId { get; set; }

    public int? PoliklinikId { get; set; }

    public int? DurumId { get; set; }

    public DateOnly RandevuTarihi { get; set; }

    public TimeOnly RandevuSaati { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Randevudurumlari? Durum { get; set; }

    public virtual ICollection<Muayeneler> Muayenelers { get; set; } = new List<Muayeneler>();

    public virtual Poliklinikler? Poliklinik { get; set; }
}
