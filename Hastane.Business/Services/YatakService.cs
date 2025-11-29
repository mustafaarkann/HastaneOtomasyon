using Hastane.DataAccess.Contexts;
using Hastane.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Hastane.Business.Services
{
    public class YatakService
    {
        private readonly HastaneContext _context;

        public YatakService(HastaneContext context)
        {
            _context = context;
        }

        // Tüm Odaları ve İçindeki Yatakları Getir
        public List<Odalar> OdalariGetir()
        {
            // Include(x => x.Yataklars) komutu kritik! 
            // Odayı getirirken yataklarını da paket yapıp getirir.
            return _context.Odalars
                           .Include(x => x.Yataklars)
                           .OrderBy(x => x.OdaNumarasi)
                           .ToList();
        }

        // YENİ: Hasta Yatış İşlemi (Veritabanına ekler, Trigger yatağı doldurur)
        public void YatisVer(Yatislar yatis)
        {
            // KONTROL: Bu hasta şu an başka bir yatakta yatıyor mu?
            // (Çıkış tarihi NULL olan bir kaydı var mı?)
            var zatenYatiyorMu = _context.Yatislars.Any(x => x.HastaTc == yatis.HastaTc && x.CikisTarihi == null);

            if (zatenYatiyorMu)
            {
                throw new Exception("Bu hasta şu an hastanede zaten yatışta görünüyor! Aynı anda iki yatak verilemez.");
            }

            _context.Yatislars.Add(yatis);
            _context.SaveChanges();
        }

        // YENİ: Taburcu Et (Önce Yatak ID'den aktif yatış kaydını bulur, sonra SP çağırır)
        // GÜNCELLENMİŞ TABURCU ET (Hastayı Komple Siler)
        public void TaburcuEt(int yatakId)
        {
            // 1. Bu yatakta yatan aktif kaydı bul
            var aktifYatis = _context.Yatislars
                                     .FirstOrDefault(x => x.YatakId == yatakId && x.CikisTarihi == null);

            if (aktifYatis != null)
            {
                string hastaTc = aktifYatis.HastaTc;

                // 2. ÖNCE YATAĞI BOŞALT (SQL Prosedürü ile)
                // Bunu yapmazsak yatak dolu kalır!
                _context.Database.ExecuteSqlRaw("SELECT sp_HastaTaburcuEt({0})", aktifYatis.YatisId);

                // 3. HASTAYI SİSTEMDEN SİLMEK İÇİN TEMİZLİK
                // Silmeden önce bağlı olduğu kayıtları temizlememiz lazım yoksa veritabanı hata verir (Foreign Key).

                // A) Hastanın Randevularını Sil
                var randevular = _context.Randevulars.Where(x => x.HastaTc == hastaTc).ToList();
                _context.Randevulars.RemoveRange(randevular);

                // B) Hastanın Yatış Geçmişini Sil (Şu anki dahil)
                var yatislar = _context.Yatislars.Where(x => x.HastaTc == hastaTc).ToList();
                _context.Yatislars.RemoveRange(yatislar);

                // C) Hastanın Muayene/İşlem kayıtları varsa onları da silmek gerekebilir
                // (Şimdilik basit tutuyoruz)

                _context.SaveChanges(); // Önce yan kayıtları silmeyi onayla

                // 4. SON OLARAK HASTAYI SİL
                var hasta = _context.Kisilers.FirstOrDefault(x => x.TcNo == hastaTc);
                if (hasta != null)
                {
                    _context.Kisilers.Remove(hasta);
                    _context.SaveChanges();
                }
            }
        }


    }
}