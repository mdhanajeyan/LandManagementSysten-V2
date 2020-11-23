using LandBankManagement.Models;
using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace LandBankManagement.Views 
{ 
    public sealed partial class VendorCard : UserControl
    {
        public VendorCard()
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
