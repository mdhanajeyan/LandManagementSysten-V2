using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using LandBankManagement.Converters;
using LandBankManagement.Services;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PropertyDetails : UserControl
    {
        public INavigationService NavigationService { get; }
        public PropertyDetails()
        {
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
        }
        #region ViewModel
        public PropertyDetailsViewModel ViewModel
        {
            get { return (PropertyDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PropertyDetailsViewModel), typeof(PropertyDetails), new PropertyMetadata(null));
        #endregion

        public void SetFocus()
        {
            details.SetFocus();
        }

        private void partySearch_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GetParties();
        }

        private void AddParty_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PreparePartyList();
        }

        private void Party_Delete_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.RemoveParty(identity);
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
        private string IsNullOrEnpty(string val)
        {
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
                    ViewModel.EditableItem.LandAreaInputGuntas = "0";
                    return;
                }
                if (anas > 16 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Land Area Anas Shoud not be greater than 16");
                    ViewModel.EditableItem.LandAreaInputAanas = "0";
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
                    ViewModel.EditableItem.AKarabAreaInputGuntas ="0";
                    return;
                }
                if (anas > 16 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Akarab Anas Shoud not be greater than 16");
                    ViewModel.EditableItem.AKarabAreaInputAanas = "0";
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
                    ViewModel.EditableItem.BKarabAreaInputGuntas = "0";
                    return;
                }
                if (anas > 16 && !IsAnyContentDialogOpen())
                {
                    await ViewModel.ValidationMeassge("Bkarab Anas Shoud not be greater than 16");
                    ViewModel.EditableItem.BKarabAreaInputAanas = "0";
                    return;
                }
                var result = AreaConvertor.ConvertArea(area, guntas, anas);
                ViewModel.loadAcres(result, "BKarab");
            }
        }

        private void Doc_Delete_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.DeleteDocument(identity);
        }

        private void AddNewPropertyBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CloneProperty();
        }

        private void SelectedPropertyInGroup_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.LoadPropertyById(id);
        }

        private void SurveyNoTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.PreparePropertyName();
        }

        private void Add_NewParty_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.storeItems();
             NavigationService.Navigate(typeof(PartyViewModel), new PartyListArgs{ SelectedPageIndex=1,FromProperty=true});
           
        }

        private void Doc_Dpwnload_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.DownloadDocument(identity);

        }

        private void DocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var identity = Convert.ToInt32(((ComboBox)sender).SelectedValue.ToString());
            ViewModel.SetCurrentDocumentType(identity);
        }

        private void uploadBtn_Click(object sender, RoutedEventArgs e)
        {
            var identity = ((Button)sender).Tag.ToString();
            if (ViewModel.Item.DocumentTypeId!=identity)
                ViewModel.ShiftDocumentType(Convert.ToInt32(identity));
            ViewModel.OnEditFile();
        }

        private void changeDocType_Click(object sender, RoutedEventArgs e)
        {

            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.ShiftDocumentType(identity);
        }

        private void primaryParty_Checked(object sender, RoutedEventArgs e)
        {
            var partyName = ((RadioButton)sender).Tag.ToString();
            ViewModel.UpdatePropertyName(partyName);
        }

        private async void TalukDDl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null|| val.ToString()=="0")
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

        private void VillageDDL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null)
                return;
           // ViewModel.Item.VillageId = val.ToString();
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
    }
}
