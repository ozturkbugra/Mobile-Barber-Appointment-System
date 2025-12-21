using Microsoft.Extensions.DependencyInjection;

namespace BerberAsistani
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Temayı güvenli bir şekilde zorla
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // MainPage atamasını burada yapmak en sağlıklı yoldur
            return new Window(new AppShell());
        }
    }
}