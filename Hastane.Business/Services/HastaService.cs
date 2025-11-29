using Hastane.DataAccess.Contexts;
using Hastane.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Hastane.Business.Services
{
    public class HastaService
    {
        private readonly HastaneContext _context;

        public HastaService(HastaneContext context)
        {
            _context = context;
        }

        // 1. Tüm Hastaları Listele
        public List<Hastalar> TumHastalar()
        {
            return _context.Hastalars.OrderByDescending(x => x.KisiId).ToList();
        }

        // 2. Yeni Hasta Ekle
        // 2. Yeni Hasta Ekle (GÜNCELLENMİŞ VERSİYON)
        public void HastaEkle(Hastalar hasta)
        {
            // 1. Bu TC ile kayıtlı herhangi bir kişi (Doktor/Personel) var mı?
            var mevcutKisi = _context.Kisilers.FirstOrDefault(x => x.TcNo == hasta.TcNo);

            if (mevcutKisi != null)
            {
                // KİŞİ ZATEN VAR! (Demek ki Doktor veya Personel)
                // O zaman yeni kişi yaratmayacağız, var olan kişiyi 'Hastalar' tablosuna bağlayacağız.

                // Önce bu kişi zaten hasta mı diye bakalım?
                var zatenHastaMi = _context.Hastalars.Any(x => x.KisiId == mevcutKisi.KisiId);
                if (zatenHastaMi)
                {
                    throw new Exception("Bu kişi zaten hasta listesinde kayıtlı!");
                }

                // Kişi var ama hasta değil. O zaman sadece HASTA rolünü ekleyelim.
                // EF Core modellerini ayırdığımız için burada Raw SQL kullanmak en temiz yoldur.
                string sql = @"INSERT INTO Hastalar (kisi_id, kan_grubu, sosial_guvence, boy, kilo) 
                               VALUES ({0}, {1}, {2}, {3}, {4})";

                _context.Database.ExecuteSqlRaw(sql,
                    mevcutKisi.KisiId,
                    hasta.KanGrubu,
                    hasta.SosyalGuvence,
                    hasta.Boy ?? 0, // Null ise 0
                    hasta.Kilo ?? 0
                );
            }
            else
            {
                // KİŞİ YOK, SIFIRDAN KAYIT (Eski yöntem)
                hasta.Sifre = "1234";
                hasta.KullaniciTuru = "Hasta";
                _context.Hastalars.Add(hasta);
                _context.SaveChanges();
            }
        }

        // 3. Hasta Sil (Opsiyonel)
        public void HastaSil(int id)
        {
            var hasta = _context.Hastalars.Find(id);
            if (hasta != null)
            {
                _context.Hastalars.Remove(hasta);
                _context.SaveChanges();
            }
        }

        // 4. TC Numarası Kontrolü (Zaten var mı?)
        public bool TcKimlikVarMi(string tcNo)
        {
            // Kisiler tablosuna bakıyoruz çünkü TC tüm sistemde (Doktor/Hasta) benzersiz olmalı
            return _context.Kisilers.Any(x => x.TcNo == tcNo);
        }
    }
}