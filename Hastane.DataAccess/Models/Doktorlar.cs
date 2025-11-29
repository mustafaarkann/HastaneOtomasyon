using System;
using System.Collections.Generic;

namespace Hastane.DataAccess.Models;

public partial class Doktorlar // : Kisiler SİLDİK
{
    // --- ORTAK ÖZELLİKLER ---
    public int KisiId { get; set; }
    public string TcNo { get; set; } = null!;
    public string Ad { get; set; } = null!;
    public string Soyad { get; set; } = null!;
    public string? Telefon { get; set; }
    public string? Cinsiyet { get; set; }
    public string? DogumYeri { get; set; }
    public string Sifre { get; set; } = null!;
    public string? KullaniciTuru { get; set; }
    // ------------------------

    public string? UzmanlikAlani { get; set; }
    public string? DiplomaNo { get; set; }
}