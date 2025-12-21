using SQLite;
using BerberAsistani.Models;

namespace BerberAsistani.Services
{
    public class RandevuService
    {
        private SQLiteAsyncConnection _db;

        async Task Init()
        {
            if (_db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "BerberDbV2.db3"); // Dosya adını değiştirdim temiz başlangıç olsun
            _db = new SQLiteAsyncConnection(databasePath);
            await _db.CreateTableAsync<Randevu>();
        }

        // Belirli bir günün randevularını getir (Saat sırasına göre)
        public async Task<List<Randevu>> GetGunlukRandevular(DateTime tarih)
        {
            await Init();
            var gunBasi = tarih.Date;
            var gunSonu = tarih.Date.AddDays(1).AddTicks(-1);

            return await _db.Table<Randevu>()
                            .Where(r => r.Baslangic >= gunBasi && r.Baslangic <= gunSonu)
                            .OrderBy(r => r.Baslangic) // Saate göre sırala
                            .ToListAsync();
        }

        // ÇAKIŞMA KONTROLÜ: Yeni eklenecek saatte başka müşteri var mı?
        public async Task<bool> CakismaVarMi(DateTime baslangic, DateTime bitis)
        {
            await Init();

            // Mantık: 
            // Yeni Başlangıç < Eski Bitiş VE Yeni Bitiş > Eski Başlangıç
            // Bu formül evrenseldir, her türlü çakışmayı yakalar.
            var cakisma = await _db.Table<Randevu>()
                                   .Where(r => baslangic < r.Bitis && bitis > r.Baslangic)
                                   .CountAsync();

            return cakisma > 0; // 0'dan büyükse çakışma var demektir
        }

        public async Task AddRandevu(Randevu randevu)
        {
            await Init();
            await _db.InsertAsync(randevu);
        }

        public async Task DeleteRandevu(Randevu randevu)
        {
            await Init();
            await _db.DeleteAsync(randevu);
        }
    }
}