using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views { 
    public sealed partial class PartiesDetails : UserControl
    {
        public PartiesDetails()
        {
            this.InitializeComponent();
        }
    #region ViewModel
    public PartyDetailsViewModel ViewModel
    {
        get { return (PartyDetailsViewModel)GetValue(ViewModelProperty); }
        set { SetValue(ViewModelProperty, value); }
    }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(PartyDetailsViewModel), typeof(PartiesDetails), new PropertyMetadata(null));
    #endregion
}
}
