using LandBankManagement.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class CompanyPane : UserControl
    {
        public CompanyPane()
        {
            this.InitializeComponent();
        }

        #region ItemsSource
        public IList<CompanyModel> ItemsSource
        {
            get { return (IList<CompanyModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<CompanyModel>), typeof(CompanyPane), new PropertyMetadata(null));
        #endregion
    }
}
