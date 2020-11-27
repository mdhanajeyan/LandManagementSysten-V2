using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class VillageList : UserControl
    {
        public VillageList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public VillageListViewModel ViewModel
        {
            get { return (VillageListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(VillageListViewModel), typeof(VillageList), new PropertyMetadata(null));
        #endregion
    }
}
