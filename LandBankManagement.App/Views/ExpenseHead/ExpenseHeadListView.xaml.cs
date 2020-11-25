using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class ExpenseHeadListView : UserControl
    {
        public ExpenseHeadListView()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public ExpenseHeadListViewModel ViewModel
        {
            get { return (ExpenseHeadListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ExpenseHeadListViewModel), typeof(ExpenseHeadListView), new PropertyMetadata(null));
        #endregion
    }
}
