using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class ReceiptsDetails : UserControl
    {
        public ReceiptsDetails()
        {
            this.InitializeComponent();
        }
        public ReceiptsDetailsViewModel ViewModel
        {
            get { return (ReceiptsDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ReceiptsDetailsViewModel), typeof(ReceiptsDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }

        private void DealPartiesDDl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.LoadDealParties();
        }
    }
}
