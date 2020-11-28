using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PropertyTypeList : UserControl
    {
        public PropertyTypeList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public PropertyTypeListViewModel ViewModel
        {
            get { return (PropertyTypeListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PropertyTypeListViewModel), typeof(PropertyTypeList), new PropertyMetadata(null));
        #endregion
    }
}
