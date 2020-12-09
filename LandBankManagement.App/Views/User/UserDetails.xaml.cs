using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class UserDetails : UserControl
    {
        public UserDetails()
        {
            this.InitializeComponent();
        }

        public UserDetailsViewModel ViewModel
        {
            get { return (UserDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(UserDetailsViewModel), typeof(PaymentDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }

    }
}
