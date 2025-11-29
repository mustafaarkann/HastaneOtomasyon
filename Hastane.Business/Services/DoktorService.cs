using Hastane.DataAccess.Contexts;
using Hastane.DataAccess.Models;

namespace Hastane.Business.Services
{
    public class DoktorService
    {
        private readonly HastaneContext _context;

        public DoktorService(HastaneContext context)
        {
            _context = context;
        }

        // 1. Tüm Doktorları Listele
        public List<Doktorlar> TumDoktorlar()
        {
            return _context.Doktorlars.OrderBy(x => x.Ad).ToList();
        }

        // 2. Yeni Doktor Ekle
        public void DoktorEkle(Doktorlar doktor)
        {
            // Doktor sisteme "Doktor" rolüyle ve varsayılan şifreyle kaydedilir
            doktor.KullaniciTuru = "Doktor";
            doktor.Sifre = "1234"; // Doktor ilk girişte bunu kullanacak

            _context.Doktorlars.Add(doktor);
            _context.SaveChanges();
        }

        // 3. ID'ye Göre Doktor Getir (Düzenleme sayfasına veriyi doldurmak için)
        public Doktorlar? DoktorGetir(int id)
        {
            return _context.Doktorlars.Find(id);
        }

        // 4. Doktoru Güncelle
        public void DoktorGuncelle(Doktorlar doktor)
        {
            _context.Doktorlars.Update(doktor);
            _context.SaveChanges();
        }

        // 5. İsme Göre Doktor Ara
        public List<Doktorlar> DoktorAra(string arananKelime)
        {
            // Büyük/Küçük harf duyarlılığı olmasın diye ToLower() kullanabiliriz ama 
            // PostgreSQL'de ILIKE daha doğrudur. Şimdilik basit Contains yapalım.
            return _context.Doktorlars
                           .Where(x => x.Ad.Contains(arananKelime) || x.Soyad.Contains(arananKelime))
                           .ToList();
        }

        // 6. TC Numarası Evrensel Kontrolü (Hasta/Personel/Doktor fark etmeksizin bakar)
        public bool TcKimlikVarMi(string tcNo)
        {
            // Kisiler tablosuna soruyoruz. Bu tablo herkesi kapsar.
            return _context.Kisilers.Any(x => x.TcNo == tcNo);
        }
    }
}