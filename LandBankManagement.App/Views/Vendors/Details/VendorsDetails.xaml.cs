using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

namespace LandBankManagement.Views
{
    public sealed partial class VendorsDetails : UserControl
    {
        public VendorsDetails()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public VendorDetailsViewModel ViewModel
        {
            get { return (VendorDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(VendorDetailsViewModel), typeof(VendorsDetails), new PropertyMetadata(null));
        #endregion
    }
}
