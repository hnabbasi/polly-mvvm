using PollyMVVM.Services.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using PollyMVVM.Models;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;

namespace PollyMVVM.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        readonly IStatesService _statesService;

        public MainPageViewModel(INavigationService navigationService, IStatesService statesService)
            : base(navigationService)
        {
            Title = "States";
            _statesService = statesService;

            InitializeCommands();
        }

		public DelegateCommand LoadStatesCommand { get; private set; }
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
            ClearCommand = new DelegateCommand(OnClearTapped);
        }

        void OnClearTapped()
        {
            States = null;
        }

        async void OnLoadStatesTapped()
        {
            IsBusy = true;
            await LoadStates();
            IsBusy = false;
        }

        async Task LoadStates()
        {
            await Task.Delay(2000);
            States = new ObservableCollection<State>(await _statesService.GetStates());
        }
	}
}
