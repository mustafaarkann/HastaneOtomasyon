using Hastane.Business.Services;
using Hastane.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastane.Web.Controllers
{
    [Authorize(Roles = "Sekreter")] // Sadece Sekreter
    public class HastaController : Controller
    {
        private readonly HastaService _hastaService;

        public HastaController(HastaService hastaService)
        {
            _hastaService = hastaService;
        }

        public IActionResult Index()
        {
            var hastalar = _hastaService.TumHastalar();
            return View(hastalar);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Hastalar hasta)
        {
            try
            {
                // Form validasyonlarını temizle (Şifre/Tür)
                ModelState.Remove("Sifre");
                ModelState.Remove("KullaniciTuru");

                if (!ModelState.IsValid)
                {
                    ViewBag.Hata = "Lütfen tüm zorunlu alanları doldurunuz.";
                    return View(hasta);
                }

                // --- YENİ EKLENEN KISIM: TC KONTROLÜ ---
                if (_hastaService.TcKimlikVarMi(hasta.TcNo))
                {
                    ViewBag.Hata = "Bu TC Kimlik numarası ile sistemde zaten kayıtlı bir kişi var! Aynı TC ile yeni kayıt açılamaz.";
                    return View(hasta); // Formu hata mesajıyla geri döndür
                }
                // ---------------------------------------   

                _hastaService.HastaEkle(hasta);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string hataMesaji = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Hata = "Kayıt Başarısız: " + hataMesaji;
                return View(hasta);
            }
        }

        public IActionResult Delete(int id)
        {
            _hastaService.HastaSil(id);
            return RedirectToAction("Index");
        }
    }
}