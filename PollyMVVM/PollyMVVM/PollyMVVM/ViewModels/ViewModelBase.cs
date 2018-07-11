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

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

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
    }
}
