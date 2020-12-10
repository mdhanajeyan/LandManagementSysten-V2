using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using LandBankManagement.Converters;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class CostDetails : UserControl
    {
        public CostDetails()
        {
            this.InitializeComponent();
        }

        public CostDetailsViewModel ViewModel
        {
            get { return (CostDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CostDetailsViewModel), typeof(CostDetails), new PropertyMetadata(null));

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyListViewModel.PopupOpened = false;
           
        }

        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddPaymentToList();
            TotalAmountTxt1.Text = ViewModel.TotalAmount1;
            TotalAmountTxt2.Text = ViewModel.TotalAmount2;
        }

        private void ClearPayment_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearPayment();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SavePaymentSequence();
        }

        private void SaleValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ViewModel.Item.SaleValue1) && !string.IsNullOrEmpty(ViewModel.Item.SaleValue1)) {

                var valu = Convert.ToDecimal(ViewModel.Item.SaleValue1) + Convert.ToDecimal(ViewModel.Item.SaleValue2);
                if (valu > 0) {
                    TotalSales.Text = valu.ToString();
                }
            }

        }
    }
}
