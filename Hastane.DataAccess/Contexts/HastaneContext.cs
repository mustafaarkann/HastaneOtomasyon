using System;
using System.Collections.Generic;
using Hastane.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Hastane.DataAccess.Contexts;

public partial class HastaneContext : DbContext
{
    public HastaneContext()
    {
    }

    public HastaneContext(DbContextOptions<HastaneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doktorlar> Doktorlars { get; set; }

    public virtual DbSet<Hastaislemleri> Hastaislemleris { get; set; }

    public virtual DbSet<Hastalar> Hastalars { get; set; }

    public virtual DbSet<Islemler> Islemlers { get; set; }

    public virtual DbSet<Islemloglari> Islemloglaris { get; set; }

    public virtual DbSet<Kasahareketleri> Kasahareketleris { get; set; }

    public virtual DbSet<Kisiler> Kisilers { get; set; }

    public virtual DbSet<Muayeneler> Muayenelers { get; set; }

    public virtual DbSet<Odalar> Odalars { get; set; }

    public virtual DbSet<Personeller> Personellers { get; set; }

    public virtual DbSet<Poliklinikler> Polikliniklers { get; set; }

    public virtual DbSet<Randevudurumlari> Randevudurumlaris { get; set; }

    public virtual DbSet<Randevular> Randevulars { get; set; }

    public virtual DbSet<Yataklar> Yataklars { get; set; }

    public virtual DbSet<Yatislar> Yatislars { get; set; }

    // NOT: Bu satırı appsettings.json'dan okuyacağı için boş bırakabilirsin veya böyle kalabilir.
    // Ancak appsettings.json ayarın varsa burası ezilir.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=HastaneOtomasyonDB;Username=postgres;Password=004Arkan.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. ANA TABLO (Parent)
        modelBuilder.Entity<Kisiler>(entity =>
        {
            entity.HasKey(e => e.KisiId).HasName("kisiler_pkey");
            entity.ToTable("kisiler");

            entity.Property(e => e.KisiId).HasColumnName("kisi_id").HasDefaultValueSql("nextval('kisiler_kisi_id_seq'::regclass)");
            entity.Property(e => e.TcNo).HasColumnName("tc_no").HasMaxLength(11);
            entity.Property(e => e.Ad).HasColumnName("ad").HasMaxLength(50);
            entity.Property(e => e.Soyad).HasColumnName("soyad").HasMaxLength(50);
            entity.Property(e => e.Telefon).HasColumnName("telefon").HasMaxLength(15);
            entity.Property(e => e.Cinsiyet).HasColumnName("cinsiyet").HasMaxLength(5);
            entity.Property(e => e.Sifre).HasColumnName("sifre").HasMaxLength(50);
            entity.Property(e => e.KullaniciTuru).HasColumnName("kullanici_turu").HasMaxLength(20);
            entity.Property(e => e.DogumYeri).HasColumnName("dogum_yeri").HasMaxLength(50);

            entity.HasIndex(e => e.TcNo, "kisiler_tc_no_key").IsUnique();
        });

        // 2. ÇOCUK TABLOLAR (DÜZELTİLDİ: HasBaseType kalktı, alanlar eklendi)
        // Böylece EF Core "TC No boş olamaz" hatası vermeyecek.

        // DOKTORLAR
        modelBuilder.Entity<Doktorlar>(entity =>
        {
            entity.HasKey(e => e.KisiId); // <--- BU SATIR EKSİKTİ, EKLE!
            entity.ToTable("doktorlar");

            // Ortak Alanlar
            entity.Property(e => e.KisiId).HasColumnName("kisi_id"); // ID maplendi
            entity.Property(e => e.TcNo).HasColumnName("tc_no").HasMaxLength(11);
            entity.Property(e => e.Ad).HasColumnName("ad").HasMaxLength(50);
            entity.Property(e => e.Soyad).HasColumnName("soyad").HasMaxLength(50);
            entity.Property(e => e.Telefon).HasColumnName("telefon").HasMaxLength(15);
            entity.Property(e => e.Cinsiyet).HasColumnName("cinsiyet").HasMaxLength(5);
            entity.Property(e => e.Sifre).HasColumnName("sifre").HasMaxLength(50);
            entity.Property(e => e.KullaniciTuru).HasColumnName("kullanici_turu").HasMaxLength(20);
            entity.Property(e => e.DogumYeri).HasColumnName("dogum_yeri").HasMaxLength(50);

            // Özel Alanlar
            entity.Property(e => e.UzmanlikAlani).HasColumnName("uzmanlik_alani").HasMaxLength(50);
            entity.Property(e => e.DiplomaNo).HasColumnName("diploma_no").HasMaxLength(20);
        });
        // HASTALAR
        modelBuilder.Entity<Hastalar>(entity =>
        {
            entity.HasKey(e => e.KisiId); // <--- BU SATIR EKSİKTİ, EKLE!
            entity.ToTable("hastalar");

            // Ortak Alanlar
            entity.Property(e => e.KisiId).HasColumnName("kisi_id");
            entity.Property(e => e.TcNo).HasColumnName("tc_no").HasMaxLength(11);
            entity.Property(e => e.Ad).HasColumnName("ad").HasMaxLength(50);
            entity.Property(e => e.Soyad).HasColumnName("soyad").HasMaxLength(50);
            entity.Property(e => e.Telefon).HasColumnName("telefon").HasMaxLength(15);
            entity.Property(e => e.Cinsiyet).HasColumnName("cinsiyet").HasMaxLength(5);
            entity.Property(e => e.Sifre).HasColumnName("sifre").HasMaxLength(50);
            entity.Property(e => e.KullaniciTuru).HasColumnName("kullanici_turu").HasMaxLength(20);
            entity.Property(e => e.DogumYeri).HasColumnName("dogum_yeri").HasMaxLength(50);

            // Özel Alanlar
            entity.Property(e => e.KanGrubu).HasColumnName("kan_grubu").HasMaxLength(10);
            entity.Property(e => e.SosyalGuvence).HasColumnName("sosyal_guvence").HasMaxLength(50);
            entity.Property(e => e.Boy).HasColumnName("boy");
            entity.Property(e => e.Kilo).HasColumnName("kilo");
        });

        // PERSONELLER
        modelBuilder.Entity<Personeller>(entity =>
        {
            entity.HasKey(e => e.KisiId); // <--- BU SATIR EKSİKTİ, EKLE!
            entity.ToTable("personeller");

            // Ortak Alanlar
            entity.Property(e => e.KisiId).HasColumnName("kisi_id");
            entity.Property(e => e.TcNo).HasColumnName("tc_no").HasMaxLength(11);
            entity.Property(e => e.Ad).HasColumnName("ad").HasMaxLength(50);
            entity.Property(e => e.Soyad).HasColumnName("soyad").HasMaxLength(50);
            entity.Property(e => e.Telefon).HasColumnName("telefon").HasMaxLength(15);
            entity.Property(e => e.Cinsiyet).HasColumnName("cinsiyet").HasMaxLength(5);
            entity.Property(e => e.Sifre).HasColumnName("sifre").HasMaxLength(50);
            entity.Property(e => e.KullaniciTuru).HasColumnName("kullanici_turu").HasMaxLength(20);
            entity.Property(e => e.DogumYeri).HasColumnName("dogum_yeri").HasMaxLength(50);

            // Özel Alanlar
            entity.Property(e => e.Maas).HasColumnName("maas").HasPrecision(10, 2);
            entity.Property(e => e.Gorev).HasColumnName("gorev").HasMaxLength(50);
            entity.Property(e => e.DepartmanId).HasColumnName("departman_id");
        });
        // 3. DİĞER TABLOLAR (Standart Ayarlar)
        modelBuilder.Entity<Hastaislemleri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hastaislemleri_pkey");
            entity.ToTable("hastaislemleri");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adet).HasColumnName("adet").HasDefaultValue(1);
            entity.Property(e => e.IslemId).HasColumnName("islem_id");
            entity.Property(e => e.MuayeneId).HasColumnName("muayene_id");
            entity.Property(e => e.IslemTarihi).HasColumnName("islem_tarihi").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Islem).WithMany(p => p.Hastaislemleris)
                .HasForeignKey(d => d.IslemId).HasConstraintName("hastaislemleri_islem_id_fkey");

            entity.HasOne(d => d.Muayene).WithMany(p => p.Hastaislemleris)
                .HasForeignKey(d => d.MuayeneId).HasConstraintName("hastaislemleri_muayene_id_fkey");
        });

        modelBuilder.Entity<Islemler>(entity =>
        {
            entity.HasKey(e => e.IslemId).HasName("islemler_pkey");
            entity.ToTable("islemler");
            entity.Property(e => e.IslemId).HasColumnName("islem_id");
            entity.Property(e => e.IslemAdi).HasColumnName("islem_adi").HasMaxLength(100);
            entity.Property(e => e.Ucret).HasColumnName("ucret").HasPrecision(10, 2);
        });

        modelBuilder.Entity<Islemloglari>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("islemloglari_pkey");
            entity.ToTable("islemloglari");
            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.IslemTuru).HasColumnName("islem_turu").HasMaxLength(50);
            entity.Property(e => e.Aciklama).HasColumnName("aciklama");
            entity.Property(e => e.KullaniciTc).HasColumnName("kullanici_tc").HasMaxLength(11);
            entity.Property(e => e.LogTarihi).HasColumnName("log_tarihi").HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Kasahareketleri>(entity =>
        {
            entity.HasKey(e => e.HareketId).HasName("kasahareketleri_pkey");
            entity.ToTable("kasahareketleri");
            entity.Property(e => e.HareketId).HasColumnName("hareket_id");
            entity.Property(e => e.Tutar).HasColumnName("tutar").HasPrecision(10, 2);
            entity.Property(e => e.Turu).HasColumnName("turu").HasMaxLength(10);
            entity.Property(e => e.Aciklama).HasColumnName("aciklama").HasMaxLength(255);
            entity.Property(e => e.Tarih).HasColumnName("tarih").HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Muayeneler>(entity =>
        {
            entity.HasKey(e => e.MuayeneId).HasName("muayeneler_pkey");
            entity.ToTable("muayeneler");
            entity.Property(e => e.MuayeneId).HasColumnName("muayene_id");
            entity.Property(e => e.Teshis).HasColumnName("teshis");
            entity.Property(e => e.DoktorNotu).HasColumnName("doktor_notu");
            entity.Property(e => e.ReceteNo).HasColumnName("recete_no").HasMaxLength(20);
            entity.Property(e => e.RandevuId).HasColumnName("randevu_id");

            entity.HasOne(d => d.Randevu).WithMany(p => p.Muayenelers)
                .HasForeignKey(d => d.RandevuId).HasConstraintName("muayeneler_randevu_id_fkey");
        });

        modelBuilder.Entity<Odalar>(entity =>
        {
            entity.HasKey(e => e.OdaId).HasName("odalar_pkey");
            entity.ToTable("odalar");
            entity.Property(e => e.OdaId).HasColumnName("oda_id");
            entity.Property(e => e.OdaNumarasi).HasColumnName("oda_numarasi").HasMaxLength(10);
            entity.Property(e => e.Kat).HasColumnName("kat");
            entity.Property(e => e.Kapasite).HasColumnName("kapasite");
        });

        modelBuilder.Entity<Poliklinikler>(entity =>
        {
            entity.HasKey(e => e.PoliklinikId).HasName("poliklinikler_pkey");
            entity.ToTable("poliklinikler");
            entity.Property(e => e.PoliklinikId).HasColumnName("poliklinik_id");
            entity.Property(e => e.PoliklinikAdi).HasColumnName("poliklinik_adi").HasMaxLength(50);
            entity.Property(e => e.BinaNo).HasColumnName("bina_no").HasMaxLength(10);
            entity.Property(e => e.KatNo).HasColumnName("kat_no");
        });

        modelBuilder.Entity<Randevudurumlari>(entity =>
        {
            entity.HasKey(e => e.DurumId).HasName("randevudurumlari_pkey");
            entity.ToTable("randevudurumlari");
            entity.Property(e => e.DurumId).HasColumnName("durum_id");
            entity.Property(e => e.DurumAdi).HasColumnName("durum_adi").HasMaxLength(20);
        });

        modelBuilder.Entity<Randevular>(entity =>
        {
            entity.HasKey(e => e.RandevuId).HasName("randevular_pkey");
            entity.ToTable("randevular");
            entity.Property(e => e.RandevuId).HasColumnName("randevu_id");
            entity.Property(e => e.HastaTc).HasColumnName("hasta_tc").HasMaxLength(11);
            entity.Property(e => e.DoktorId).HasColumnName("doktor_id");
            entity.Property(e => e.PoliklinikId).HasColumnName("poliklinik_id");
            entity.Property(e => e.DurumId).HasColumnName("durum_id");
            entity.Property(e => e.RandevuTarihi).HasColumnName("randevu_tarihi");
            entity.Property(e => e.RandevuSaati).HasColumnName("randevu_saati");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Poliklinik).WithMany(p => p.Randevulars)
                .HasForeignKey(d => d.PoliklinikId).HasConstraintName("randevular_poliklinik_id_fkey");

            entity.HasOne(d => d.Durum).WithMany(p => p.Randevulars)
                .HasForeignKey(d => d.DurumId).HasConstraintName("randevular_durum_id_fkey");
        });

        modelBuilder.Entity<Yataklar>(entity =>
        {
            entity.HasKey(e => e.YatakId).HasName("yataklar_pkey");
            entity.ToTable("yataklar");
            entity.Property(e => e.YatakId).HasColumnName("yatak_id");
            entity.Property(e => e.YatakNo).HasColumnName("yatak_no").HasMaxLength(10);
            entity.Property(e => e.DoluMu).HasColumnName("dolu_mu").HasDefaultValue(false);
            entity.Property(e => e.OdaId).HasColumnName("oda_id");

            entity.HasOne(d => d.Oda).WithMany(p => p.Yataklars)
                .HasForeignKey(d => d.OdaId).HasConstraintName("yataklar_oda_id_fkey");
        });

        modelBuilder.Entity<Yatislar>(entity =>
        {
            entity.HasKey(e => e.YatisId).HasName("yatislar_pkey");
            entity.ToTable("yatislar");
            entity.Property(e => e.YatisId).HasColumnName("yatis_id");
            entity.Property(e => e.HastaTc).HasColumnName("hasta_tc").HasMaxLength(11);
            entity.Property(e => e.YatakId).HasColumnName("yatak_id");
            entity.Property(e => e.GirisTarihi).HasColumnName("giris_tarihi").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CikisTarihi).HasColumnName("cikis_tarihi");

            entity.HasOne(d => d.Yatak).WithMany(p => p.Yatislars)
                .HasForeignKey(d => d.YatakId).HasConstraintName("yatislar_yatak_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}