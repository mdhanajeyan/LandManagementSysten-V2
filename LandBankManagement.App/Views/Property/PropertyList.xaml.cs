using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PropertyList : UserControl
    {
        public PropertyList()
        {
            this.InitializeComponent();
        }
        public PropertyListViewModel ViewModel
        {
            get { return (PropertyListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PropertyListViewModel), typeof(PropertyList), new PropertyMetadata(null));
    }
}
