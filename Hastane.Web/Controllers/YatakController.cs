using Hastane.Business.Services;
using Hastane.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli

namespace Hastane.Web.Controllers
{
    [Authorize(Roles = "Sekreter")]
    public class YatakController : Controller
    {
        private readonly YatakService _yatakService;
        private readonly HastaService _hastaService; // YENİ: Hasta Servisini Ekledik

        // Constructor'ı Güncelledik (İki servisi de alıyor)
        public YatakController(YatakService yatakService, HastaService hastaService)
        {
            _yatakService = yatakService;
            _hastaService = hastaService;
        }

        public IActionResult Index()
        {
            var odalar = _yatakService.OdalariGetir();
            return View(odalar);
        }

        // 1. YATIŞ SAYFASINI GÖSTER (GET) - GÜNCELLENDİ
        [HttpGet]
        public IActionResult YatisVer(int id) // id = YatakId
        {
            // A. Yatak Bilgisini Güzelleştirelim (Oda No'yu bulalım)
            var secilenYatak = _yatakService.OdalariGetir()
                .SelectMany(o => o.Yataklars, (oda, yatak) => new { Oda = oda, Yatak = yatak })
                .FirstOrDefault(x => x.Yatak.YatakId == id);

            if (secilenYatak != null)
            {
                ViewBag.YatakBilgisi = $"Oda: {secilenYatak.Oda.OdaNumarasi} / Yatak No: {secilenYatak.Yatak.YatakNo}";
            }
            else
            {
                ViewBag.YatakBilgisi = "Yatak ID: " + id;
            }

            ViewBag.YatakId = id;

            // B. Hasta Listesini Hazırla (TC - Ad Soyad formatında)
            var hastaListesi = _hastaService.TumHastalar()
                .Select(x => new
                {
                    TcNo = x.TcNo,
                    Gorunum = $"{x.TcNo} - {x.Ad} {x.Soyad}"
                }).ToList();

            ViewBag.Hastalar = new SelectList(hastaListesi, "TcNo", "Gorunum");

            return View();
        }

        // 2. YATIŞ İŞLEMİNİ YAP (POST) - AYNI KALIYOR
        [HttpPost]
        public IActionResult YatisVer(Yatislar yatis)
        {
            try
            {
                _yatakService.YatisVer(yatis);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "Hata: " + ex.Message;
                // Hata olursa listeyi tekrar doldurmamız lazım yoksa sayfa patlar
                var hastaListesi = _hastaService.TumHastalar()
                    .Select(x => new { TcNo = x.TcNo, Gorunum = $"{x.TcNo} - {x.Ad} {x.Soyad}" }).ToList();
                ViewBag.Hastalar = new SelectList(hastaListesi, "TcNo", "Gorunum");

                ViewBag.YatakId = yatis.YatakId;
                ViewBag.YatakBilgisi = "Seçilen Yatak (Tekrar Deneyin)";

                return View();
            }
        }

        // 3. TABURCU ETME İŞLEMİ
        public IActionResult TaburcuEt(int id)
        {
            _yatakService.TaburcuEt(id);
            return RedirectToAction("Index");
        }
    }
}