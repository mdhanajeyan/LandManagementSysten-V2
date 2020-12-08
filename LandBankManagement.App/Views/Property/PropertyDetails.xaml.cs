using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

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
    }
}
