using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Randevudurumlari
{
    public int DurumId { get; set; }

    public string? DurumAdi { get; set; }

    public virtual ICollection<Randevular> Randevulars { get; set; } = new List<Randevular>();
}
