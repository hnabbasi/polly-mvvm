using PollyMVVM.Services.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PollyMVVM.Models;
using System;
using System.Threading.Tasks;
using Prism.Events;

namespace PollyMVVM.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        readonly ICountriesService _countriesService;
        readonly IPageDialogService _pageDialogService;
        readonly IEventAggregator _eventAggregator;

        public MainPageViewModel(INavigationService navigationService, ICountriesService countriesService, IPageDialogService pageDialogService,
                                 IEventAggregator eventAggregator)
            : base(navigationService)
        {
            Title = "Countries";
            _countriesService = countriesService;
            _pageDialogService = pageDialogService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<WaitRetryEvent>().Subscribe(OnRetrying);
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

        void OnRetrying(int retryCount)
        {
            // if retrying for the second time, let the user know politely
            if (retryCount > 1)
            {
                LoadingText = "Still loading...";
            }
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
                Countries = shouldWaitAndRetry ? await _countriesService.GetCountriesWithWaitAndRetry() : await _countriesService.GetCountriesWithRetry();
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
