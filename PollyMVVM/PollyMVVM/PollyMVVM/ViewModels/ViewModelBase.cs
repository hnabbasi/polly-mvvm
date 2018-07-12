using Prism.Mvvm;
using Prism.Navigation;

namespace PollyMVVM.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        const string DEFAULT_LOADING = "Loading...";

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        string _loadingText = DEFAULT_LOADING;
        public string LoadingText
        {
            get { return _loadingText; }
            set { SetProperty(ref _loadingText, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        protected void ShowLoading(string message = null)
        {
            IsBusy = true;
            LoadingText = message ?? DEFAULT_LOADING;
        }

        protected void DismissLoading()
        {
            IsBusy = false;
        }

        public void OnNavigatedFrom(INavigationParameters parameters) { }

        public void OnNavigatedTo(INavigationParameters parameters) { }

        public void OnNavigatingTo(INavigationParameters parameters) { }

        public void Destroy() { }
    }
}
