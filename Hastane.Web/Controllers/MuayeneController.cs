using Hastane.Business.Services;
using Hastane.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastane.Web.Controllers
{
    [Authorize]
    public class MuayeneController : Controller
    {
        private readonly MuayeneService _muayeneService;

        public MuayeneController(MuayeneService muayeneService)
        {
            _muayeneService = muayeneService;
        }

        // 1. MUAYENE FORMU (GET)
        [HttpGet]
        public IActionResult MuayeneEt(int randevuId)
        {
            ViewBag.RandevuId = randevuId;
            // İşlemleri Checkbox listesi yapmak için gönderiyoruz
            ViewBag.Islemler = _muayeneService.IslemleriGetir();
            return View();
        }

        [HttpPost]
        public IActionResult MuayeneEt(Muayeneler muayene, int[] secilenIslemler)
        {
            try
            {
                ModelState.Clear(); // Validasyonu geç

                // Eğer RandevuId 0 geldiyse manuel hata fırlat (En sık yapılan hata)
                if (muayene.RandevuId == 0)
                {
                    // View'den hidden input ile gelmemiştir belki diye formdan okumayı dene
                    if (Request.Form.ContainsKey("RandevuId"))
                    {
                        muayene.RandevuId = int.Parse(Request.Form["RandevuId"]);
                    }
                    else
                    {
                        throw new Exception("Randevu ID bulunamadı! Sayfadaki gizli kutucuk (hidden input) boş.");
                    }
                }

                _muayeneService.MuayeneKaydet(muayene, secilenIslemler);

                return RedirectToAction("Index", "Randevu");
            }
            catch (Exception ex)
            {
                // --- HATAYI DETAYLANDIRAN KISIM ---
                string hataDetayi = ex.Message;

                // Varsa iç hatayı ekle
                if (ex.InnerException != null)
                {
                    hataDetayi += " || DETAY: " + ex.InnerException.Message;

                    // Varsa onun da içini ekle (PostgreSQL hataları genelde buradadır)
                    if (ex.InnerException.InnerException != null)
                    {
                        hataDetayi += " || ALT DETAY: " + ex.InnerException.InnerException.Message;
                    }
                }
                // ----------------------------------

                ViewBag.Hata = hataDetayi;

                // Sayfa verilerini tekrar doldur
                ViewBag.RandevuId = muayene.RandevuId;
                ViewBag.Islemler = _muayeneService.IslemleriGetir();
                return View(muayene);
            }
        }

        // 3. KASA RAPORU SAYFASI
        public IActionResult Kasa()
        {
            var kasaHareketleri = _muayeneService.KasaRaporuGetir();

            // Toplam Bakiyeyi Hesapla
            ViewBag.ToplamBakiye = kasaHareketleri.Sum(x => x.Turu == "Gelir" ? x.Tutar : -x.Tutar);

            return View(kasaHareketleri);
        }
    }
}