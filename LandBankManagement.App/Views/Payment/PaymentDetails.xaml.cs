using LandBankManagement.ViewModels;
using System;
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

        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {

            ViewModel.AddPaymentToList();
        }

        private void ClearPayment_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearCurrentPayment();
        }

        private void Doc_Delete_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.DeletePaymentList(identity);
        }
    }
}
