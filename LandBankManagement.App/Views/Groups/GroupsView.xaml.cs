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
    public sealed partial class GroupsView : Page
    {
        public GroupsViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }
        public GroupsView()
        {
            ViewModel = ServiceLocator.Current.GetService<GroupsViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
            ViewModel.GroupsDetials.IsEditMode = true;
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            await ViewModel.LoadAsync(e.Parameter as GroupsListArgs);          
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {           

        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }
    }
}
