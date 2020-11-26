using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class TalukList : UserControl
    {
        public TalukList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public TalukListViewModel ViewModel
        {
            get { return (TalukListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(TalukListViewModel), typeof(TalukList), new PropertyMetadata(null));
        #endregion
    }
}
