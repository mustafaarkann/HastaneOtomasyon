using Hastane.Business.Services;
using Hastane.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hastane.Web.Controllers
{
    [Authorize] // Giriş yapmayan giremez
    public class RandevuController : Controller
    {
        private readonly RandevuService _randevuService;

        public RandevuController(RandevuService randevuService)
        {
            _randevuService = randevuService;
        }

        // LİSTELEME SAYFASI
        public IActionResult Index()
        {
            var randevular = _randevuService.TumRandevulariGetir();
            return View(randevular);
        }

        // EKLEME SAYFASI (Formu Göster)
        [HttpGet]
        public IActionResult Create()
        {
            // Dropdownları doldur (ViewBag ile sayfaya taşıyoruz)
            ViewBag.Poliklinikler = new SelectList(_randevuService.PoliklinikleriGetir(), "PoliklinikId", "PoliklinikAdi");

            // Doktorları Ad+Soyad birleştirerek listeye atalım
            var doktorlar = _randevuService.DoktorlariGetir()
                .Select(d => new {
                    Id = d.KisiId,
                    AdSoyad = d.Ad + " " + d.Soyad + " (" + d.UzmanlikAlani + ")"
                }).ToList();

            ViewBag.Doktorlar = new SelectList(doktorlar, "Id", "AdSoyad");

            return View();
        }

        // EKLEME İŞLEMİ (Kaydet Butonuna Basınca)
        [HttpPost]
        public IActionResult Create(Randevular randevu)
        {
            try
            {
                // Varsayılan değerler
                randevu.DurumId = 1; // 1: Bekliyor

                _randevuService.RandevuEkle(randevu); // Kaydetmeyi dene

                return RedirectToAction("Index"); // Başarılıysa listeye dön
            }
            catch (Exception ex)
            {
                // HATA YAKALANDI! (Trigger hatası olabilir)
                // Hatayı ekrana basacağız
                ViewBag.Hata = "Hata oluştu: " + ex.InnerException?.Message ?? ex.Message;

                // Dropdownları tekrar doldur ki sayfa bozulmasın
                ViewBag.Poliklinikler = new SelectList(_randevuService.PoliklinikleriGetir(), "PoliklinikId", "PoliklinikAdi");
                var doktorlar = _randevuService.DoktorlariGetir()
                    .Select(d => new { Id = d.KisiId, AdSoyad = d.Ad + " " + d.Soyad }).ToList();
                ViewBag.Doktorlar = new SelectList(doktorlar, "Id", "AdSoyad");

                return View(randevu);
            }
        }

        // ONAYLA BUTONUNA BASINCA
        public IActionResult Onayla(int id)
        {
            _randevuService.RandevuOnayla(id);
            return RedirectToAction("Index"); // Listeyi yenile
        }

        // İPTAL BUTONUNA BASINCA
        public IActionResult Iptal(int id)
        {
            _randevuService.RandevuIptal(id);
            return RedirectToAction("Index"); // Listeyi yenile
        }

        // AJAX İLE ÇAĞRILACAK METOT
        [HttpGet]
        public JsonResult GetDoktorlarByPoliklinik(int poliklinikId)
        {
            var doktorlar = _randevuService.DoktorlariGetirByPoliklinik(poliklinikId)
                .Select(d => new
                {
                    value = d.KisiId,
                    text = d.Ad + " " + d.Soyad + " (" + d.UzmanlikAlani + ")"
                });

            return Json(doktorlar);
        }
    }
}