using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class ReceiptsList : UserControl
    {
        public ReceiptsList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public ReceiptsListViewModel ViewModel
        {
            get { return (ReceiptsListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ReceiptsListViewModel), typeof(ReceiptsList), new PropertyMetadata(null));
        #endregion
    }
}
