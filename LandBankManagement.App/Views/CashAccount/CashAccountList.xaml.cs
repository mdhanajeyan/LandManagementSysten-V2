using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class CashAccountList : UserControl
    {
        public CashAccountList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public CashAccountListViewModel ViewModel
        {
            get { return (CashAccountListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CashAccountListViewModel), typeof(CashAccountList), new PropertyMetadata(null));
        #endregion
    }
}
