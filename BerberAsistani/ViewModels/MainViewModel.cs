using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BerberAsistani.Models;
using BerberAsistani.Services;

namespace BerberAsistani.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly RandevuService _service;
        private DateTime _secilenTarih;

        // Direkt gerçek randevuları tutuyoruz
        public ObservableCollection<Randevu> Randevular { get; set; } = new();

        public MainViewModel(RandevuService service)
        {
            _service = service;
            SecilenTarih = DateTime.Now;
        }

        public DateTime SecilenTarih
        {
            get => _secilenTarih;
            set
            {
                _secilenTarih = value;
                OnPropertyChanged();
                ListeyiYukle();
            }
        }

        public async void ListeyiYukle()
        {
            Randevular.Clear();
            var liste = await _service.GetGunlukRandevular(SecilenTarih);
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