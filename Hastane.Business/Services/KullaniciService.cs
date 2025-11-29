using Hastane.DataAccess.Contexts;
using Hastane.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hastane.Business.Services
{
    public class KullaniciService
    {
        private readonly HastaneContext _context;

        // Constructor (Veritabanı bağlantısını buraya çağırıyoruz)
        public KullaniciService(HastaneContext context)
        {
            _context = context;
        }

        // Giriş Kontrol Metodu
        public Kisiler? GirisYap(string tcNo, string sifre)
        {
            // Veritabanına git, TC ve Şifresi uyan ilk kişiyi getir.
            // Kisiler tablosuna soruyoruz ama EF Core otomatik olarak alt tabloları da tarar.
            var kullanici = _context.Kisilers
                                    .FirstOrDefault(x => x.TcNo == tcNo && x.Sifre == sifre);

            return kullanici; // Kullanıcı yoksa null döner.
        }
    }
}