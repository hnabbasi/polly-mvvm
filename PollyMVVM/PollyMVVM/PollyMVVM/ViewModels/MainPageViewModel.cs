using PollyMVVM.Services.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PollyMVVM.Models;
using System;
using System.Threading.Tasks;

namespace PollyMVVM.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        readonly ICountriesService _countriesService;
        private readonly IPageDialogService _pageDialogService;

        public MainPageViewModel(INavigationService navigationService, ICountriesService countriesService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Countries";
            _countriesService = countriesService;
            _pageDialogService = pageDialogService;
            InitializeCommands();
        }

        public DelegateCommand LoadCountriesCommand { get; private set; }
        public DelegateCommand<object> LoadCountriesRetryCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }

        Country[] _countries;
        public Country[] Countries
        {
            get { return _countries; }
            set { SetProperty(ref _countries, value); }
        }

        void InitializeCommands()
        {
            LoadCountriesCommand = new DelegateCommand(OnLoadCountriesTapped);
            LoadCountriesRetryCommand = new DelegateCommand<object>(OnLoadCountriesRetryTapped);
            ClearCommand = new DelegateCommand(OnClearTapped);
        }

        void OnClearTapped()
        {
            Countries = null;
        }

        async void OnLoadCountriesTapped()
        {
            ShowLoading();
            await LoadCountries();
            DismissLoading();
        }

        async void OnLoadCountriesRetryTapped(object shouldWaitAndRetry)
        {
            ShowLoading();
            await LoadCountriesWithRetry(shouldWaitAndRetry.ToString() == "true");
            DismissLoading();
        }

        async Task LoadCountries()
        {
            try
            {
                Countries = await _countriesService.GetCountries();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ShowAlert("Could Not Load Countries. Try again later.");
            }
        }

        async Task LoadCountriesWithRetry(bool shouldWaitAndRetry)
        {
            try
            {
                var getStatesTask = shouldWaitAndRetry ? _countriesService.GetCountriesWithWaitAndRetry() : _countriesService.GetCountriesWithRetry();
                Countries = await getStatesTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ShowAlert("Could Not Load Countries. Try again later.");
            }
        }

        void ShowAlert(string message)
        {
            _pageDialogService.DisplayAlertAsync("Oops...", message, "OK");
        }
    }
}
