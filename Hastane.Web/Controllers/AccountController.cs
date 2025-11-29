using Hastane.Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hastane.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly KullaniciService _kullaniciService;

        public AccountController(KullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Login sayfasını göster
        }

        [HttpPost]
        public async Task<IActionResult> Login(string tcNo, string sifre)
        {
            // SADECE VE SADECE BU BİLGİLERLE GİRİŞ YAPILSIN
            if (tcNo == "11111111111" && sifre == "123")
            {
                // Giriş Başarılı!
                // Şimdi sisteme "Bu giren kişi Sekreterdir" diye sahte kimlik verelim
                // ki menüler açılsın.

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Yönetici Sekreter"), // Üstte yazacak isim
                    new Claim("TcNo", "11111111111"),
                    new Claim(ClaimTypes.Role, "Sekreter") // Rolü Sekreter olsun ki menüleri görsün
                };

                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "CookieAuth"));

                // Cookie oluştur ve içeri al
                await HttpContext.SignInAsync("CookieAuth", userPrincipal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Başka ne yazılırsa yazılsın HATA ver
                ViewBag.Hata = "Hatalı Giriş! Sadece yetkili personel girebilir.";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}