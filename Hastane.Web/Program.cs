using Microsoft.EntityFrameworkCore;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Veritabaný Baðlantýsý (Dependency Injection)
var connectionString = builder.Configuration.GetConnectionString("HastaneBaglantisi");
builder.Services.AddDbContext<Hastane.DataAccess.Contexts.HastaneContext>(options =>
    options.UseNpgsql(connectionString));

// Servisleri Tanýt (Dependency Injection)
builder.Services.AddScoped<Hastane.Business.Services.KullaniciService>();
builder.Services.AddScoped<Hastane.Business.Services.RandevuService>();
builder.Services.AddScoped<Hastane.Business.Services.YatakService>();
builder.Services.AddScoped<Hastane.Business.Services.HastaService>();
builder.Services.AddScoped<Hastane.Business.Services.DoktorService>();
builder.Services.AddScoped<Hastane.Business.Services.MuayeneService>();

// Authentication (Giriþ yetkilendirme) servisini aç
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.LoginPath = "/Account/Login"; // Yetkisiz giriþ yapaný buraya at
        config.Cookie.Name = "HastaneOtomasyon.Cookie";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
