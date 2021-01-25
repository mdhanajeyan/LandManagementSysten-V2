using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;
using System;
using Syncfusion.UI.Xaml.TreeGrid;
using System.Collections.Generic;
using LandBankManagement.Models;
using System.Linq;
using LandBankManagement.Controls;
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
        #region Query
        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register(nameof(Query), typeof(string), typeof(PropertyList), new PropertyMetadata(null));
        #endregion
        public ListToolbarMode ToolbarMode =>  ListToolbarMode.Default;

        private void CostDetails_Click(object sender, RoutedEventArgs e)
        {
            var propertyId = Convert.ToInt32(((Button)sender).Tag.ToString());
             ViewModel.CostDetails.LoadAsync(propertyId);
            ViewModel.PopupOpened = true;
        }

        private void Popup_closeBtn_Click(object sender, RoutedEventArgs e)
        {
            CostDetailsPopup.IsOpen = false;
        }

        private void DocumentType_Click(object sender, RoutedEventArgs e)
        {
            var propertyId = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.PropertyView.LoadPropertyForNewDocumentType(propertyId);
            
        }
               
        private void treeGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            //var item = (PropertyModel)treeGrid.SelectedItem;
            //ViewModel.PopulateProperty(item);
        }

        private void EditProperty_Click(object sender, RoutedEventArgs e)
        {
            var propertyId = Convert.ToInt32(((Button)sender).Tag.ToString());
            var selectedItem = ViewModel.Items.Where(x => x.PropertyId == propertyId).FirstOrDefault();
            ViewModel.PopulateProperty(selectedItem);
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
                   // NewCommand?.TryExecute();
                    break;
                case ToolbarButton.Delete:
                  //  DeleteCommand?.TryExecute();
                    break;
                case ToolbarButton.Select:
                    //StartSelectionCommand?.TryExecute();
                    break;
                case ToolbarButton.Refresh:
                    // RefreshCommand?.TryExecute();
                   await ViewModel.RefreshAsync();
                    break;
                case ToolbarButton.Cancel:
                   // CancelSelectionCommand?.TryExecute();
                    break;
            }
        }

        private void Popup_DrogBtn_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            scalreTransform.ScaleX *= e.Delta.Scale;
            scalreTransform.ScaleY *= e.Delta.Scale;
            translateTransform.X += e.Delta.Translation.X;
            translateTransform.Y += e.Delta.Translation.Y;
        }

        private void ToggleRow_Click(object sender, RoutedEventArgs e)
        {
            var index = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.ChangeVisibility(index);
        }
    }
}
