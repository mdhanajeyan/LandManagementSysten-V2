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
            ViewModel.AddPropertyToList();
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

        private void CompanyDDL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.LoadPropertyOptionByCompany();
        }

        private async void PropertyDDL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            await  ViewModel.GetDocumentType();
        }

        private async void DocumentTypeDDL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null)
                return;
            ViewModel.selectedDocumentType = Convert.ToInt32(val);
            await ViewModel.LoadedSelectedProperty();
        }
    }
}
