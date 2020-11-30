
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

namespace LandBankManagement.Views
{
    public sealed partial class VendorList : UserControl
    {
        public VendorList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public VendorListViewModel ViewModel
        {
            get { return (VendorListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(VendorListViewModel), typeof(VendorList), new PropertyMetadata(null));
        #endregion
    }
}
