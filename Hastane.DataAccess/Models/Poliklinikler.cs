using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Poliklinikler
{
    public int PoliklinikId { get; set; }

    public string? PoliklinikAdi { get; set; }

    public string? BinaNo { get; set; }

    public int? KatNo { get; set; }

    public virtual ICollection<Randevular> Randevulars { get; set; } = new List<Randevular>();
}
