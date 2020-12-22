﻿using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using LandBankManagement.Converters;
using LandBankManagement.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PropertyCheckListDetails : UserControl
    {
        public INavigationService NavigationService { get; }
        public PropertyCheckListDetails()
        {
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
        }
        #region ViewModel
        public PropertyCheckListDetailsViewModel ViewModel
        {
            get { return (PropertyCheckListDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PropertyCheckListDetailsViewModel), typeof(PropertyCheckListDetails), new PropertyMetadata(null));
        #endregion

        public void SetFocus()
        {
            details.SetFocus();
        }

        private void partySearch_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GetVendors();
        }

        private void AddParty_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PrepareVendorList();
        }

        private void Party_Delete_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.RemoveVendor(identity);
        }

        private void AreaTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ConvertArea("Area");
        }
        private void Akarab_LostFocus(object sender, RoutedEventArgs e)
        {
            ConvertArea("AKarab");
        }
        private void Bkarab_LostFocus(object sender, RoutedEventArgs e)
        {
            ConvertArea("BKarab");
        }

        private string IsNullOrEnpty(string val) {
            return string.IsNullOrEmpty(val) ? "0" : val;
        }
        public void ConvertArea(string type)
        {
            if (type == "Area")
            {               
                    var area = Convert.ToDecimal(IsNullOrEnpty( ViewModel.EditableItem.LandAreaInputAcres));
                    var guntas = Convert.ToDecimal(IsNullOrEnpty( ViewModel.EditableItem.LandAreaInputGuntas));
                    var anas= Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.LandAreaInputAanas));
                    var result = AreaConvertor.ConvertArea(area, guntas,anas);
                    ViewModel.loadAcres(result, "Area");            
            }
            if (type == "AKarab")
            {                
                    var area = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.AKarabAreaInputAcres));
                    var guntas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.AKarabAreaInputGuntas));
                    var anas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.AKarabAreaInputAanas));
                    var result = AreaConvertor.ConvertArea(area, guntas,anas);
                    ViewModel.loadAcres(result, "AKarab");               
            }
            if (type == "BKarab")
            {               
                    var area = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.BKarabAreaInputAcres));
                    var guntas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.BKarabAreaInputGuntas));
                    var anas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.BKarabAreaInputAanas));
                    var result = AreaConvertor.ConvertArea(area, guntas,anas);
                    ViewModel.loadAcres(result, "BKarab");               
            }
        }

        private void Doc_Delete_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.DeleteDocument(identity);
        }

        private void SurveyNoTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.PreparePropertyName();
        }

        private void SelectAllCheckList_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            var isCheck = (bool)((CheckBox)sender).IsChecked;
            ViewModel.SelectAllCheckList(isCheck);
        }

        private void Add_vendor_Click(object sender, RoutedEventArgs e)
        {
             NavigationService.Navigate(typeof(VendorViewModel) ,new VendorListArgs { SelectedPageIndex=1});
        }
    }
}
