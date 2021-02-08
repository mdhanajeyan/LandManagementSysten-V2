using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class GroupsList : UserControl
    {
        public GroupsList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public GroupsListViewModel ViewModel
        {
            get { return (GroupsListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(GroupsListViewModel), typeof(GroupsList), new PropertyMetadata(null));
        #endregion
    }
}
