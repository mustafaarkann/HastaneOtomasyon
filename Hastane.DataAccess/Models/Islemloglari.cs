using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Islemloglari
{
    public int LogId { get; set; }

    public DateTime? LogTarihi { get; set; }

    public string? IslemTuru { get; set; }

    public string? Aciklama { get; set; }

    public string? KullaniciTc { get; set; }
}
