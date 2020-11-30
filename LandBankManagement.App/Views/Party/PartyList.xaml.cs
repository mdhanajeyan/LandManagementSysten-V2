
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

namespace LandBankManagement.Views
{
    public sealed partial class PartyList : UserControl
    {
        public PartyList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public PartyListViewModel ViewModel
        {
            get { return (PartyListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PartyListViewModel), typeof(PartyList), new PropertyMetadata(null));
        #endregion
    }
}
