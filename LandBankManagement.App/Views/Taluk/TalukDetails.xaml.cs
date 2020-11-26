using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class TalukDetails : UserControl
    {
        public TalukDetails()
        {
            this.InitializeComponent();
        }
        public TalukDetailsViewModel ViewModel
        {
            get { return (TalukDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TalukDetailsViewModel), typeof(TalukDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
