using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PaymentList : UserControl
    {
        public PaymentList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public PaymentsListViewModel ViewModel
        {
            get { return (PaymentsListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PaymentsListViewModel), typeof(PaymentList), new PropertyMetadata(null));
        #endregion
    }
}
