using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Hastaislemleri
{
    public int Id { get; set; }

    public int? MuayeneId { get; set; }

    public int? IslemId { get; set; }

    public int? Adet { get; set; }

    public DateTime? IslemTarihi { get; set; }

    public virtual Islemler? Islem { get; set; }

    public virtual Muayeneler? Muayene { get; set; }
}
