using BerberAsistani.Models;
using BerberAsistani.Services;
using BerberAsistani.ViewModels;

namespace BerberAsistani;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    private readonly RandevuService _service;

    public MainPage(RandevuService service)
    {
        InitializeComponent();
        _service = service;
        _viewModel = new MainViewModel(_service);
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ListeyiYukle();
    }

    private async void OnEkleClicked(object sender, EventArgs e)
    {
        // 1. İsim İste
        string ad = await DisplayPromptAsync("Yeni Randevu", "Müşteri Adı:");
        if (string.IsNullOrWhiteSpace(ad)) return;

        // 2. İşlem İste
        string islem = await DisplayActionSheet("İşlem Seç", "İptal", null, "Saç", "Sakal", "Saç + Sakal", "Yıkama/Fön");
        if (islem == "İptal" || islem == null) return;

        // 3. Saat Seçtirme (Basitlik için TimePicker kullanamıyoruz, Prompt ile alalım veya özel ekran gerekir)
        // Hızlı çözüm için kullanıcıya string olarak saat soruyoruz, sonra parse ediyoruz.
        // Daha profesyoneli için özel popup yapılır ama şimdilik en hızlısı bu:

        // Başlangıç Saati?
        string saatStr = await DisplayPromptAsync("Saat Kaçta?", "Örn: 14:30", initialValue: DateTime.Now.ToString("HH:mm"));
        if (!TimeSpan.TryParse(saatStr, out TimeSpan baslangicSaati))
        {
            await DisplayAlert("Hata", "Geçersiz saat formatı!", "Tamam");
            return;
        }

        // Kaç Dakika Sürecek?
        string sureStr = await DisplayActionSheet("Ne Kadar Sürecek?", "İptal", null, "15 Dk", "30 Dk", "45 Dk", "60 Dk", "90 Dk");
        if (sureStr == "İptal" || sureStr == null) return;
        int dakika = int.Parse(sureStr.Split(' ')[0]); // "30 Dk" -> 30

        // Tarih ve Saati Birleştir
        DateTime baslangicTarihi = _viewModel.SecilenTarih.Date + baslangicSaati;
        DateTime bitisTarihi = baslangicTarihi.AddMinutes(dakika);

        // ÇAKIŞMA KONTROLÜ
        bool cakismaVar = await _service.CakismaVarMi(baslangicTarihi, bitisTarihi);
        if (cakismaVar)
        {
            await DisplayAlert("DOLU!", "Bu saat aralığında (veya bir kısmında) koltuk dolu. Lütfen başka saat seç.", "Tamam");
            return;
        }

        // Kaydet
        var yeni = new Randevu
        {
            AdSoyad = ad,
            Islem = islem,
            Baslangic = baslangicTarihi,
            Bitis = bitisTarihi
        };

        await _service.AddRandevu(yeni);
        _viewModel.ListeyiYukle();
    }

    private async void OnSilClicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        var randevu = btn.CommandParameter as Randevu;

        bool cevap = await DisplayAlert("Sil", $"{randevu.AdSoyad} silinsin mi?", "Evet", "Hayır");
        if (cevap)
        {
            await _service.DeleteRandevu(randevu);
            _viewModel.ListeyiYukle();
        }
    }
}