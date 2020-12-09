using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

namespace LandBankManagement.Views
{
    public sealed partial class UserList : UserControl
    {
        public UserList()
        {
            this.InitializeComponent();
        }
        public UserListViewModel ViewModel
        {
            get { return (UserListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(UserListViewModel), typeof(UserList), new PropertyMetadata(null));

    }
}
