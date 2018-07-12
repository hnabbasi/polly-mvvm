using Prism;
using Prism.Ioc;
using PollyMVVM.ViewModels;
using PollyMVVM.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using PollyMVVM.Services.Abstractions;
using PollyMVVM.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PollyMVVM
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();

            containerRegistry.Register<INetworkService, NetworkService>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IClient, Client>();
            containerRegistry.Register<ICountriesService, CountriesService>();
        }
    }
}
