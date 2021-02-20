using LandBankManagement.Common;
using LandBankManagement.Controls;
using LandBankManagement.Models;
using LandBankManagement.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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


        private async void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Query = args.QueryText;
            await ViewModel.RefreshAsync();
            //  QuerySubmittedCommand?.TryExecute(args.QueryText);
        }
        private async void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {

            switch (e.ClickedButton)
            {
                case ToolbarButton.New:
                    break;
                case ToolbarButton.Delete:
                    break;
                case ToolbarButton.Select:
                    break;
                case ToolbarButton.Refresh:
                    await ViewModel.RefreshAsync();
                    break;
                case ToolbarButton.Cancel:
                    break;
            }
        }

        private void sfdataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            var model = (PropertyMergeModel)sfdataGrid.SelectedItem;
            if (model != null)
                ViewModel.OnSelectedRow(model);
        }
    }
}
