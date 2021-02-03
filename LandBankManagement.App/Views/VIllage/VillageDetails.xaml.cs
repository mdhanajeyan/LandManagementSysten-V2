using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class VillageDetails : UserControl
    {
        public VillageDetails()
        {
            this.InitializeComponent();
        }
        public VillageDetailsViewModel ViewModel
        {
            get { return (VillageDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(VillageDetailsViewModel), typeof(VillageDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }

        private void ChangeHobli_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetHobliOption(null);
        }

        private void ChangeTaluk_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetTalukOption();
        }

        private async void TalukDDl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null || val.ToString() == "0")
                return;
            await ViewModel.LoadHobli();
        }
    }
}
