using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.Models;
using LandBankManagement.ViewModels;

namespace LandBankManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VendorsCard : UserControl
    {
        public VendorsCard()
        {
            this.InitializeComponent();
        }

        #region ViewModel
        public VendorDetailsViewModel ViewModel
        {
            get { return (VendorDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(VendorDetailsViewModel), typeof(VendorsCard), new PropertyMetadata(null));
        #endregion

        #region Item
        public VendorModel Item
        {
            get { return (VendorModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(VendorModel), typeof(VendorsCard), new PropertyMetadata(null));
        #endregion
    }
}
