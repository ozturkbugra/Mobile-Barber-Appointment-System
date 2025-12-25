using BerberAsistani.Models;
using BerberAsistani.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BerberAsistani.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly RandevuService _service;
        private DateTime _secilenTarih;

        // Direkt gerçek randevuları tutuyoruz
        public ObservableCollection<Randevu> Randevular { get; set; } = new();

        public ICommand GunDegistirCommand { get; }
        public MainViewModel(RandevuService service)
        {
            _service = service;

            // ESKİ HALİ (HATALI): Bu satır Property'nin 'set'ini tetikler ve ListeyiYukle'yi çalıştırır.
            // SecilenTarih = DateTime.Now; 

            // YENİ HALİ (DOĞRU): Doğrudan değişkene atama yapıyoruz. Tetikleme yok.
            _secilenTarih = DateTime.Now;

            GunDegistirCommand = new Command<int>((miktar) =>
            {
                // Bu işlem "SecilenTarih"in set bloğunu çalıştırır, 
                // dolayısıyla ListeyiYukle() otomatik devreye girer.
                SecilenTarih = SecilenTarih.AddDays(miktar);
            });
        }

        public DateTime SecilenTarih
        {
            get => _secilenTarih;
            set
            {
                _secilenTarih = value;
                OnPropertyChanged();

                // İŞTE BURASI: Tarih değiştiği an listeyi otomatik yeniliyor.
                ListeyiYukle();
            }
        }

        public async void ListeyiYukle()
        {
            // 1. Önce veritabanına gidip veriyi alalım (Listeye dokunmuyoruz)
            var liste = await _service.GetGunlukRandevular(SecilenTarih);

            // 2. Veri elimize ulaştıktan sonra listeyi temizliyoruz
            // Böylece önceki istekler ekleme yapmış olsa bile hepsini silip en tazesini yazarız.
            Randevular.Clear();

            // 3. Şimdi ekliyoruz
            foreach (var item in liste)
            {
                Randevular.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}