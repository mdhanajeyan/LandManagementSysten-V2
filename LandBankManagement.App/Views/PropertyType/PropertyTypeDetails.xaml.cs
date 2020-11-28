using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class PropertyTypeDetails : UserControl
    {
        public PropertyTypeDetails()
        {
            this.InitializeComponent();
        }
        public PropertyTypeDetailsViewModel ViewModel
        {
            get { return (PropertyTypeDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(PropertyTypeDetailsViewModel), typeof(PropertyTypeDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
