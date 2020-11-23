using LandBankManagement.Services;
using LandBankManagement.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LandBankManagement.Views
{
    public sealed partial class CompanyView : Page
    {
        public CompanyView()
        {
            ViewModel = ServiceLocator.Current.GetService<CompanyDetailsViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            InitializeComponent();
        }

        public CompanyDetailsViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            await ViewModel.LoadAsync(e.Parameter as CompanyDetailsArgs);

            if (ViewModel.IsEditMode)
            {
                await Task.Delay(100);
                details.SetFocus();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }
    }
}
