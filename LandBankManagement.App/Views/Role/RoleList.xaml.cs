using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class RoleList : UserControl
    {
        public RoleList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public RoleListViewModel ViewModel
        {
            get { return (RoleListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(RoleListViewModel), typeof(RoleList), new PropertyMetadata(null));
        #endregion
    }
}
