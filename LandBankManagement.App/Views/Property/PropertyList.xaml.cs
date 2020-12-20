﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;
using System;
using Syncfusion.UI.Xaml.TreeGrid;
using System.Collections.Generic;
using LandBankManagement.Models;
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
            var item = (PropertyModel)treeGrid.SelectedItem;
            ViewModel.PopulateProperty(item);
        }
    }
}
