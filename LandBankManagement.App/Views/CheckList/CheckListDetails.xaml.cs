using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class CheckListDetails : UserControl
    {
        public CheckListDetails()
        {
            this.InitializeComponent();
        }
        public CheckListDetailsViewModel ViewModel
        {
            get { return (CheckListDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CheckListDetailsViewModel), typeof(CheckListDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
