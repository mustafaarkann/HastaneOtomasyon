using Hastane.DataAccess.Contexts;
using Hastane.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hastane.Business.Services
{
    public class RandevuService
    {
        private readonly HastaneContext _context;

        public RandevuService(HastaneContext context)
        {
            _context = context;
        }

        // 1. Tüm Randevuları Listele (İlişkili tablolarla beraber: Include)
        public List<Randevular> TumRandevulariGetir()
        {
            return _context.Randevulars
                           .Include(r => r.Poliklinik) // Poliklinik adını getir
                           .Include(r => r.Durum)      // Durum adını getir
                           .OrderByDescending(r => r.RandevuTarihi)
                           .ToList();
        }

        // 2. Yeni Randevu Ekle
        public void RandevuEkle(Randevular randevu)
        {
            // Veritabanına ekle
            _context.Randevulars.Add(randevu);

            // Kaydet (Burada veritabanındaki TRIGGER'lar devreye girecek!)
            _context.SaveChanges();
        }

        // 3. Dropdown İçin Poliklinikleri Getir
        public List<Poliklinikler> PoliklinikleriGetir()
        {
            return _context.Polikliniklers.ToList();
        }

        // 4. Dropdown İçin Doktorları Getir
        public List<Doktorlar> DoktorlariGetir()
        {
            return _context.Doktorlars.ToList();
        }

        // 5. Randevu Onayla (Stored Procedure Çağırır)
        public void RandevuOnayla(int id)
        {
            // SQL: SELECT sp_RandevuOnayla(5);
            _context.Database.ExecuteSqlRaw("SELECT sp_RandevuOnayla({0})", id);
        }

        // 6. Randevu İptal Et (Stored Procedure Çağırır)
        public void RandevuIptal(int id)
        {
            string iptalNedeni = "Sekreter tarafından iptal edildi."; // Basitlik için varsayılan neden

            // SQL: SELECT sp_RandevuIptal(5, 'Neden');
            _context.Database.ExecuteSqlRaw("SELECT sp_RandevuIptal({0}, {1})", id, iptalNedeni);
        }

        // 7. Poliklinik ID'sine göre Doktorları Getir (AJAX İçin)
        public List<Doktorlar> DoktorlariGetirByPoliklinik(int poliklinikId)
        {
            // 1. Önce seçilen polikliniğin adını bulalım (Örn: Kardiyoloji)
            var poliklinik = _context.Polikliniklers.Find(poliklinikId);

            if (poliklinik == null) return new List<Doktorlar>();

            // 2. Uzmanlık alanı bu poliklinik ile aynı olan doktorları getir
            // (Not: Veritabanında PoliklinikAdi ile UzmanlikAlani birebir eşleşmeli)
            return _context.Doktorlars
                           .Where(x => x.UzmanlikAlani == poliklinik.PoliklinikAdi)
                           .ToList();
        }

    }
}