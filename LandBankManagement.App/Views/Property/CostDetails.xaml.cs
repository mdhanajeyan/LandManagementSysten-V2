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
        }

        private void ClearPayment_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearPayment();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SavePaymentSequence();
        }
    }
}
