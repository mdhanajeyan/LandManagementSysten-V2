using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class CashAccountDetails : UserControl
    {
        public CashAccountDetails()
        {
            this.InitializeComponent();
        }
        public CashAccountDetailsViewModel ViewModel
        {
            get { return (CashAccountDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CashAccountDetailsViewModel), typeof(CashAccountDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
