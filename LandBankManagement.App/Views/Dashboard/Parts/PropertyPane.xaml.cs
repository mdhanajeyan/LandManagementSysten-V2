using LandBankManagement.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PropertyPane : UserControl
    {
        public PropertyPane()
        {
            this.InitializeComponent();
        }

        #region ItemsSource
        public IList<PropertyModel> ItemsSource
        {
            get { return (IList<PropertyModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<PropertyModel>), typeof(PropertyPane), new PropertyMetadata(null));
        #endregion
    }
}
