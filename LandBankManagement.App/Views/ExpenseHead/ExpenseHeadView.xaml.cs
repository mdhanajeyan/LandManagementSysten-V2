
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using LandBankManagement.ViewModels;
using LandBankManagement.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class ExpenseHeadView : Page
    {

        public INavigationService NavigationService { get; }
        public ExpenseHeadView()
        {
            ViewModel = ServiceLocator.Current.GetService<ExpenseHeadViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();

        }

        public ExpenseHeadViewModel ViewModel
        {
            get { return (ExpenseHeadViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ExpenseHeadViewModel), typeof(ExpenseHeadView), new PropertyMetadata(null));

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            await ViewModel.LoadAsync(e.Parameter as ExpenseHeadArgs);
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
           var saveIdSuccess=await ViewModel.SaveItemsAsync();
            if (!saveIdSuccess)
            {
                e.Cancel = true;
                return;
            }
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }

    }
}
