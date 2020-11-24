using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.Models;
using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PartiesCard : UserControl
    {
        public PartiesCard()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public PartyDetailsViewModel ViewModel
        {
            get { return (PartyDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(PartyDetailsViewModel), typeof(PartiesCard), new PropertyMetadata(null));
        #endregion

        #region Item
        public PartyModel Item
        {
            get { return (PartyModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(PartyModel), typeof(PartiesCard), new PropertyMetadata(null));
        #endregion
    }
}
