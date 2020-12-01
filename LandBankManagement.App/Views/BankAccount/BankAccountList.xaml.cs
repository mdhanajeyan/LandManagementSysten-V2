using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class BankAccountList : UserControl
    {
        public BankAccountList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public BankAccountListViewModel ViewModel
        {
            get { return (BankAccountListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(BankAccountListViewModel), typeof(BankAccountList), new PropertyMetadata(null));
        #endregion
    }
}
