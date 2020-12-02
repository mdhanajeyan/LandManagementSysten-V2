using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace LandBankManagement.Views
{
    public sealed partial class PaymentDetails : UserControl
    {
        public PaymentDetails()
        {
            this.InitializeComponent();
        }
        public PaymentsDetailsViewModel ViewModel
        {
            get { return (PaymentsDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(PaymentsDetailsViewModel), typeof(PaymentDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
               
    }
}
