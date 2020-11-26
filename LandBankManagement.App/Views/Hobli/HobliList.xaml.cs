using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class HobliList : UserControl
    {
        public HobliList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public HobliListViewModel ViewModel
        {
            get { return (HobliListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(HobliListViewModel), typeof(HobliList), new PropertyMetadata(null));
        #endregion
    }
}
