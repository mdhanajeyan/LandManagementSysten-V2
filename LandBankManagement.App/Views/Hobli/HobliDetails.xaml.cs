using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class HobliDetails : UserControl
    {
        public HobliDetails()
        {
            this.InitializeComponent();
        }
        public HobliDetailsViewModel ViewModel
        {
            get { return (HobliDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(HobliDetailsViewModel), typeof(HobliDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
