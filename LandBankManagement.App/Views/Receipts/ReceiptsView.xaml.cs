using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using LandBankManagement.ViewModels;
using LandBankManagement.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LandBankManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReceiptsView : Page
    {
        public ReceiptsViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }
        public ReceiptsView()
        {
            ViewModel = ServiceLocator.Current.GetService<ReceiptsViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
            progressRing.IsActive = true;
            progressRing.Visibility = Visibility.Visible;
            ViewModel.ReceiptsDetials.IsEditMode = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            await ViewModel.LoadAsync(e.Parameter as ReceiptsListArgs);
            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            //await NavigationService.CreateNewViewAsync<PartiesViewModel>(ViewModel.PartyList.CreateArgs());
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {
            //ViewModel.PartyDetails.CancelEdit();

            //await NavigationService.CreateNewViewAsync<VendorDetailsViewModel>(ViewModel.PartyDetails.CreateArgs());

        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ((Pivot)sender).SelectedIndex;
            if (index == 1)
            {
                await ViewModel.ReceiptsList.LoadAsync(new ReceiptsListArgs { IsEmpty = false });
                await ViewModel.ReceiptsList.RefreshAsync();
            }
        }
    }
}
