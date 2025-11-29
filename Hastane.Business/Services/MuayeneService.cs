using Hastane.DataAccess.Contexts;
using Hastane.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Hastane.Business.Services
{
    public class MuayeneService
    {
        private readonly HastaneContext _context;

        public MuayeneService(HastaneContext context)
        {
            _context = context;
        }

        // 1. Yapılabilecek İşlemleri Listele (Dropdown için)
        public List<Islemler> IslemleriGetir()
        {
            return _context.Islemlers.ToList();
        }

        // 2. Muayene Kaydı Oluştur ve İşlemleri Ekle (GÜNCEL)
        public void MuayeneKaydet(Muayeneler muayene, int[] secilenIslemler)
        {
            // 1. Önce Muayene Kaydını Ekle
            // ID sorunu olmasın diye ID'yi 0 yapıyoruz, veritabanı kendi versin.
            muayene.MuayeneId = 0;
            _context.Muayenelers.Add(muayene);
            _context.SaveChanges(); // Burayı geçiyorsa muayene kaydolmuştur.

            // 2. İşlemleri Ekle
            if (secilenIslemler != null)
            {
                foreach (var islemId in secilenIslemler)
                {
                    var hastaIslem = new Hastaislemleri
                    {
                        MuayeneId = muayene.MuayeneId,
                        IslemId = islemId,
                        Adet = 1
                    };
                    _context.Hastaislemleris.Add(hastaIslem);
                }
                _context.SaveChanges();
            }

            // 3. KRİTİK NOKTA: Randevu Durumunu GÜNCELLE
            // Gelen RandevuId'yi kullanarak o randevuyu buluyoruz.
            var randevu = _context.Randevulars.Find(muayene.RandevuId);

            if (randevu != null)
            {
                randevu.DurumId = 4; // 4: Tamamlandı
                _context.Randevulars.Update(randevu);
                _context.SaveChanges(); // Değişikliği veritabanına yaz
            }
            else
            {
                // Eğer randevu bulunamazsa hata fırlat ki anlayalım
                throw new Exception($"Randevu ID ({muayene.RandevuId}) veritabanında bulunamadı!");
            }
        }

        // 3. Kasa Raporu (Kasahareketleri tablosu)
        public List<Kasahareketleri> KasaRaporuGetir()
        {
            return _context.Kasahareketleris.OrderByDescending(x => x.Tarih).ToList();
        }
    }
}