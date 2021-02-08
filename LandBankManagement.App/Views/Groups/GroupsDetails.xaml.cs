using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class GroupsDetails : UserControl
    {
        public GroupsDetails()
        {
            this.InitializeComponent();
        }
        public GroupsDetailsViewModel ViewModel
        {
            get { return (GroupsDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(GroupsDetailsViewModel), typeof(GroupsDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
