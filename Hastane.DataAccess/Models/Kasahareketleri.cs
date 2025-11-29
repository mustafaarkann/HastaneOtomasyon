using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Kasahareketleri
{
    public int HareketId { get; set; }

    public DateTime? Tarih { get; set; }

    public decimal? Tutar { get; set; }

    public string? Turu { get; set; }

    public string? Aciklama { get; set; }
}
