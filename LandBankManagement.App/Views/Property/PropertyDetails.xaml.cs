using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using LandBankManagement.Converters;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class PropertyDetails : UserControl
    {
        public PropertyDetails()
        {
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

        public void ConvertArea(string type)
        {
            if (type == "Area")
            {
                if (!string.IsNullOrEmpty(ViewModel.Item.LandAreaInputAcres) && !string.IsNullOrEmpty(ViewModel.Item.LandAreaInputGuntas))
                {
                    var area = Convert.ToDecimal(ViewModel.Item.LandAreaInputAcres);
                    var guntas = Convert.ToDecimal(ViewModel.Item.LandAreaInputGuntas);

                    var result = AreaConvertor.ConvertArea(area, guntas);
                    ViewModel.loadAcres(result, "Area");
                }

            }
            if (type == "AKarab")
            {
                if (!string.IsNullOrEmpty(ViewModel.Item.AKarabAreaInputAcres) && !string.IsNullOrEmpty(ViewModel.Item.AKarabAreaInputGuntas))
                {
                    var area = Convert.ToDecimal(ViewModel.Item.AKarabAreaInputAcres);
                    var guntas = Convert.ToDecimal(ViewModel.Item.AKarabAreaInputGuntas);

                    var result = AreaConvertor.ConvertArea(area, guntas);
                    ViewModel.loadAcres(result, "AKarab");
                }
            }
            if (type == "BKarab")
            {
                if (!string.IsNullOrEmpty(ViewModel.Item.BKarabAreaInputAcres) && !string.IsNullOrEmpty(ViewModel.Item.BKarabAreaInputGuntas))
                {
                    var area = Convert.ToDecimal(ViewModel.Item.BKarabAreaInputAcres);
                    var guntas = Convert.ToDecimal(ViewModel.Item.BKarabAreaInputGuntas);

                    var result = AreaConvertor.ConvertArea(area, guntas);
                    ViewModel.loadAcres(result, "BKarab");
                }
            }
        }
    }
}
