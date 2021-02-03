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

        private async void CompanyDDl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null)
                return;
           await ViewModel.LoadBankAndCompany();
        }

        private void cashCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null|| val.ToString()=="0")
                return;
            ViewModel.Item.CashAccountId = val.ToString();
        }

        private void bankCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null ||val.ToString() == "0")
                return;
            ViewModel.Item.BankAccountId = val.ToString();
        }
    }
}
