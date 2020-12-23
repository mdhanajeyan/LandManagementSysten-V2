using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;
using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PropertyMergeList : UserControl
    {
        public PropertyMergeList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public PropertyMergeListViewModel ViewModel
        {
            get { return (PropertyMergeListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PropertyMergeListViewModel), typeof(PropertyMergeList), new PropertyMetadata(null));
        #endregion

        private async void CloneProperty_Click(object sender, RoutedEventArgs e)
        {
            var propertyId = Convert.ToInt32(((Button)sender).Tag.ToString());
            await ViewModel.PropertyMergeViewModel.ClonePropertyMerge(propertyId);
            

        }
    }
}
