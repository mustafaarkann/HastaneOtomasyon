using Hastane.Business.Services;
using Hastane.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastane.Web.Controllers
{
    [Authorize(Roles = "Sekreter")] // Sadece Sekreter erişebilir
    public class DoktorController : Controller
    {
        private readonly DoktorService _doktorService;

        public DoktorController(DoktorService doktorService)
        {
            _doktorService = doktorService;
        }

        // LİSTELEME VE ARAMA
        public IActionResult Index(string p)
        {
            // Eğer arama kutusu boşsa hepsini getir
            if (string.IsNullOrEmpty(p))
            {
                var doktorlar = _doktorService.TumDoktorlar();
                return View(doktorlar);
            }
            else
            {
                // Doluysa filtrele
                var doktorlar = _doktorService.DoktorAra(p);
                return View(doktorlar);
            }
        }

        // EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Doktorlar doktor)
        {
            try
            {
                ModelState.Remove("Sifre");
                ModelState.Remove("KullaniciTuru");

                if (!ModelState.IsValid)
                {
                    ViewBag.Hata = "Lütfen tüm alanları doldurunuz.";
                    return View(doktor);
                }

                // --- YENİ EKLENEN KISIM: TC KONTROLÜ ---
                // Bu TC ile kayıtlı herhangi biri (Hasta veya Personel bile olsa) var mı?
                if (_doktorService.TcKimlikVarMi(doktor.TcNo))
                {
                    ViewBag.Hata = "Bu TC Kimlik numarası sistemde zaten kayıtlı! (Doktor, Hasta veya Personel olabilir).";
                    return View(doktor);
                }
                // ---------------------------------------

                _doktorService.DoktorEkle(doktor);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string mesaj = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Hata = "Kayıt Hatası: " + mesaj;
                return View(doktor);
            }
        }

        // DÜZENLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var doktor = _doktorService.DoktorGetir(id);
            if (doktor == null) return RedirectToAction("Index");
            return View(doktor);
        }

        // DÜZENLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Edit(Doktorlar doktor)
        {
            try
            {
                // Şifre ve Türü güncellemede değişmesin, validasyondan çıkar
                ModelState.Remove("Sifre");
                ModelState.Remove("KullaniciTuru");

                // TC ve Diploma No gibi kritik veriler genelde değiştirilmez ama
                // burada basitlik olsun diye izin veriyoruz.

                _doktorService.DoktorGuncelle(doktor);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "Güncelleme hatası: " + ex.Message;
                return View(doktor);
            }
        }
    }
}