using Hastane.DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastane.Web.Controllers
{
    [Authorize] // Sadece giriþ yapanlar burayý görebilir!
    public class HomeController : Controller
    {
        private readonly HastaneContext _context;

        public HomeController(HastaneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Eðer giren kiþi SEKRETER ise istatistikleri görsün
            if (User.IsInRole("Sekreter"))
            {
                // 1. Toplam Hasta Sayýsý
                ViewBag.ToplamHasta = _context.Hastalars.Count();

                // 2. Bugünkü Randevu Sayýsý (Tarih kontrolü)
                ViewBag.BugunkuRandevu = _context.Randevulars
                    .Where(x => x.RandevuTarihi == DateOnly.FromDateTime(DateTime.Today))
                    .Count();

                // 3. Boþ Yatak Sayýsý (Yataklar tablosundan DoluMu = false olanlar)
                ViewBag.BosYatak = _context.Yataklars
                    .Where(x => x.DoluMu == false)
                    .Count();
            }

            // Eðer DOKTOR ise kendi randevu sayýsýný görsün
            if (User.IsInRole("Doktor"))
            {
                // Doktora özel veriler buraya eklenebilir
                // Þimdilik boþ geçiyoruz, dashboard'da "Hoþgeldiniz" yazacak.
            }

            return View();
        }
    }
}