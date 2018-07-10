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
        public DelegateCommand LoadStatesRetryCommand { get; private set; }
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
            LoadStatesRetryCommand = new DelegateCommand(OnLoadStatesRetryTapped);
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

        async void OnLoadStatesRetryTapped()
        {
            ShowLoading();
            await LoadStatesWithRetry();
            DismissLoading();
        }

        async Task LoadStates()
        {
            try
            {
                States = new ObservableCollection<State>(await _statesService.GetStates());
            }
            catch (Exception ex)
            {
                ShowAlert((ex.InnerException ?? ex).Message);
            }
        }

        async Task LoadStatesWithRetry()
        {
            try
            {
                States = new ObservableCollection<State>(await _statesService.GetStatesWithRetry());
            }
            catch (Exception ex)
            {
                ShowAlert((ex.InnerException ?? ex).Message);
            }
        }

        void ShowAlert(string message)
        {
            _pageDialogService.DisplayAlertAsync("Could Not Get States", message, "OK");
        }
    }
}
