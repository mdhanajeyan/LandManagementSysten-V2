using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;
using System;

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

        private  void CostDetails_Click(object sender, RoutedEventArgs e)
        {
            //CostDetailsPopup.IsOpen = true;
            var propertyId = Convert.ToInt32(((Button)sender).Tag.ToString());
             ViewModel.CostDetails.LoadAsync(propertyId);
            ViewModel.PopupOpened = true;
        }
    }
}
