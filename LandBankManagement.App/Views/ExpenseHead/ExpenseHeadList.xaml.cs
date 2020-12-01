using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace LandBankManagement.Views
{
    public sealed partial class ExpenseHeadList : UserControl
    {
        public ExpenseHeadList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public ExpenseHeadListViewModel ViewModel
        {
            get { return (ExpenseHeadListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ExpenseHeadListViewModel), typeof(ExpenseHeadList), new PropertyMetadata(null));
        #endregion
    }
}
