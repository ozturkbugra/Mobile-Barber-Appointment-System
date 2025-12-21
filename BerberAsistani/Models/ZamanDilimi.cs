using System;
using System.Collections.Generic;
using System.Text;

namespace BerberAsistani.Models
{
    public class ZamanDilimi
    {
        public string Saat { get; set; } // Örn: "09:30"
        public string MusteriAdi { get; set; } // Doluysa isim yazar, boşsa "BOŞ" yazar
        public bool DoluMu { get; set; } // Kırmızı mı yakalım yeşil mi?
        public Color Renk { get; set; } // Kutunun rengi

        // Tıklanınca hangi veritabanı kaydına denk geldiğini bilmek için
        public Randevu GercekRandevu { get; set; }
    }
}
