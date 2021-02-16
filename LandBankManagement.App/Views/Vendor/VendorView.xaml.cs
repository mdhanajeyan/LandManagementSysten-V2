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
    public sealed partial class VendorView : Page
    {
        public VendorViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }
        public VendorView()
        {
            ViewModel = ServiceLocator.Current.GetService<VendorViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
            ViewModel.VendorDetails.IsEditMode = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            var arg = (e.Parameter as VendorListArgs);
            ViewModel.SelectedPivotIndex = arg.SelectedPageIndex;
            await ViewModel.VendorDetails.LoadAsync(arg.FromPropertyCheckList);
            await ViewModel.LoadAsync(e.Parameter as VendorListArgs);
           
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
            ViewModel.ShowProgressRing();
            var index = ((Pivot)sender).SelectedIndex;
            if (index == 0)
            {
                await ViewModel.VendorList.LoadAsync(new VendorListArgs { IsEmpty = false });
                
            }
            ViewModel.HideProgressRing();
        }
    }
}
