using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using LandBankManagement.Converters;
using LandBankManagement.Services;
using Windows.UI.Xaml.Media;
using System.Drawing;
using System.Linq;

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
        private bool IsAnyContentDialogOpen()
        {
            return VisualTreeHelper.GetOpenPopups(Window.Current).Count > 0;
        }
        public async void ConvertArea(string type)
        {
            if (type == "Area")
            {
                var area = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.LandAreaInputAcres));
                var guntas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.LandAreaInputGuntas));
                var anas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.LandAreaInputAanas));

                if (guntas > 40 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Land Area Gundas Shoud not be greater than 40");
                    ViewModel.EditableItem.LandAreaInputGuntas = "";
                    return;
                }
                if (anas >= 16 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Land Area Aanas Shoud not be greater than 16");
                    ViewModel.EditableItem.LandAreaInputAanas = "";
                    return;
                }
                var result = AreaConvertor.ConvertArea(area, guntas, anas);
                ViewModel.loadAcres(result, "Area");
            }
            if (type == "AKarab")
            {
                var area = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.AKarabAreaInputAcres));
                var guntas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.AKarabAreaInputGuntas));
                var anas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.AKarabAreaInputAanas));

                if (guntas > 40 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Akarab Gundas Shoud not be greater than 40");
                    ViewModel.EditableItem.AKarabAreaInputGuntas = "";
                    return;
                }
                if (anas >= 16 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Akarab Aanas Shoud not be greater than 16");
                    ViewModel.EditableItem.AKarabAreaInputAanas = "";
                    return;
                }
                var result = AreaConvertor.ConvertArea(area, guntas, anas);
                ViewModel.loadAcres(result, "AKarab");
            }
            if (type == "BKarab")
            {
                var area = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.BKarabAreaInputAcres));
                var guntas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.BKarabAreaInputGuntas));
                var anas = Convert.ToDecimal(IsNullOrEnpty(ViewModel.EditableItem.BKarabAreaInputAanas));
                if (guntas > 40 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Bkarab Gundas Shoud not be greater than 40");
                    ViewModel.EditableItem.BKarabAreaInputGuntas = "";
                    return;
                }
                if (anas >= 16 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Bkarab Aanas Shoud not be greater than 16");
                    ViewModel.EditableItem.BKarabAreaInputAanas = "";
                    return;
                }
                var result = AreaConvertor.ConvertArea(area, guntas, anas);
                ViewModel.loadAcres(result, "BKarab");
            }
        }

        private async void Doc_Delete_Click(object sender, RoutedEventArgs e)
        {
            var identity = ((Button)sender).Tag.ToString();
            var checkListId = Convert.ToInt32(identity.Split('_')[0]);
            var docInx = Convert.ToInt32(identity.Split('_')[1]);
            await ViewModel.DeleteDocument(checkListId, docInx);
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

        private void fromMasterRB_Checked(object sender, RoutedEventArgs e)
        {
            var type = ((RadioButton)sender).Tag.ToString();           
          
            ViewModel.LoadCheckList(type);
        }

        private async void PropertyCheckListCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           await ViewModel.LoadCheckListByProeprty();
        }

        private void uploadBtn_Click(object sender, RoutedEventArgs e)
        {
            var CheckListId = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.OnEditFile(CheckListId);
        }

        private void AddCheckListItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddNewCheckListItem();
        }

        private void CheckList_Delete_Click(object sender, RoutedEventArgs e)
        {
            var CheckListId = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.RemoveCheckListItem(CheckListId);
        }


        private void ChangeCompany_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetCompanyOption();
        }

        private void ChangeTaluk_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetTalukOption();
        }

        private void ChangeHobli_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetHobliOption(null);
        }

        private void ChangeVillage_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetVillageOption(null);
        }

        private void ResetDocType_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetDocumentTypeOption();
        }

        private async void TalukDDl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null || val.ToString() == "0")
                return;
            await ViewModel.LoadHobli();
        }

        private async void HobliDDl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null || val.ToString() == "0")
                return;
            // ViewModel.Item.HobliId = val.ToString();
            await ViewModel.LoadVillage();
        }

        private void primaryVendor_Checked(object sender, RoutedEventArgs e)
        {
          
        }

        private void partySearch_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {

        }

        private void SearchPopup_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
        }
        private void LandArea_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(x => char.IsDigit(x) || x == '.').ToArray());
            sender.SelectionStart = sender.Text.Length;

        }
    }
}
