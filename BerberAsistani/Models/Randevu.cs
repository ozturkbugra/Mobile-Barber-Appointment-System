using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BerberAsistani.Models
{
    public class Randevu
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string AdSoyad { get; set; }
        public string Islem { get; set; }

        // Artık tarih ve saat birleşik. Örn: 22.12.2025 14:30
        public DateTime Baslangic { get; set; }

        // Bitiş saati. Örn: 22.12.2025 15:15
        public DateTime Bitis { get; set; }

        // Ekranda göstermek için yardımcı özellikler (Veritabanına kaydolmaz)
        [Ignore]
        public string SaatAraligi => $"{Baslangic:HH:mm} - {Bitis:HH:mm}";

        [Ignore]
        public string TarihBilgisi => $"{Baslangic:dd.MM.yyyy}";
    }
}
