using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class BankAccountDetails : UserControl
    {
        public BankAccountDetails()
        {
            this.InitializeComponent();
        }
        public BankAccountDetailsViewModel ViewModel
        {
            get { return (BankAccountDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(BankAccountDetailsViewModel), typeof(CashAccountDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
     
    }
}
