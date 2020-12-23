using LandBankManagement.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class PropertyMergeDetails : UserControl
    {
        public PropertyMergeDetails()
        {
            this.InitializeComponent();
        }
        public PropertyMergeDetailsViewModel ViewModel
        {
            get { return (PropertyMergeDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(PropertyMergeDetailsViewModel), typeof(PropertyMergeDetailsViewModel), new PropertyMetadata(null));



        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddParopertyToList();
        }

        private void Delete_Property_Click(object sender, RoutedEventArgs e)
        {
            var identity = (Guid)((Button)sender).Tag;
             ViewModel.DeletePropertyMergeList(identity);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadedSelectedProperty();
        }
    }
}
