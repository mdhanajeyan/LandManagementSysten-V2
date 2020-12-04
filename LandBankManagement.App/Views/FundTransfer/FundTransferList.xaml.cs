using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class FundTransferList : UserControl
    {
        public FundTransferList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public FundTransferListViewModel ViewModel
        {
            get { return (FundTransferListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(FundTransferListViewModel), typeof(FundTransferList), new PropertyMetadata(null));
        #endregion
    }
}
