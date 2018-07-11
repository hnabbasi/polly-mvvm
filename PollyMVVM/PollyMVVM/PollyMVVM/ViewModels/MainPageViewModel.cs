using PollyMVVM.Services.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PollyMVVM.Models;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;

namespace PollyMVVM.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        readonly IStatesService _statesService;
        private readonly IPageDialogService _pageDialogService;

        public MainPageViewModel(INavigationService navigationService, IStatesService statesService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "States";
            _statesService = statesService;
            _pageDialogService = pageDialogService;
            InitializeCommands();
        }

        public DelegateCommand LoadStatesCommand { get; private set; }
        public DelegateCommand<object> LoadStatesRetryCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        
        ObservableCollection<State> _states;
        public ObservableCollection<State> States
        {
            get { return _states; }
            set { SetProperty(ref _states, value); }
        }

        void InitializeCommands()
        {
            LoadStatesCommand = new DelegateCommand(OnLoadStatesTapped);
            LoadStatesRetryCommand = new DelegateCommand<object>(OnLoadStatesRetryTapped);
            ClearCommand = new DelegateCommand(OnClearTapped);
        }

        void OnClearTapped()
        {
            States = null;
        }

        async void OnLoadStatesTapped()
        {
            ShowLoading();
            await LoadStates();
            DismissLoading();
        }

        async void OnLoadStatesRetryTapped(object shouldWaitAndRetry)
        {
            ShowLoading();
            await LoadStatesWithRetry(shouldWaitAndRetry.ToString() == "true");
            DismissLoading();
        }

        async Task LoadStates()
        {
            try
            {
                States = new ObservableCollection<State>(await _statesService.GetStates());
            }
            catch (Exception)
            {
                ShowAlert("Could Not Load States. Try again later.");
            }
        }

        async Task LoadStatesWithRetry(bool shouldWaitAndRetry)
        {
            try
            {
                var getStatesTask = shouldWaitAndRetry ? _statesService.GetStatesWithWaitAndRetry() : _statesService.GetStatesWithRetry();
                States = new ObservableCollection<State>(await getStatesTask);
            }
            catch (Exception)
            {
                ShowAlert("Could Not Load States. Try again later.");
            }
        }

        void ShowAlert(string message)
        {
            _pageDialogService.DisplayAlertAsync("Oops...", message, "OK");
        }
    }
}
