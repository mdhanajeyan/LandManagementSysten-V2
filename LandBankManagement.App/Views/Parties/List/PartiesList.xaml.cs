
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PartiesList : UserControl
    {
        public PartiesList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public PartyListViewModel ViewModel
        {
            get { return (PartyListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PartyListViewModel), typeof(PartiesList), new PropertyMetadata(null));
        #endregion
    }
}
