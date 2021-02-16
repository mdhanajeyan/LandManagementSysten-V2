using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using System.Linq;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class VendorDetails : UserControl
    {
        public VendorDetails()
        {
            this.InitializeComponent();
        }

        #region ViewModel
        public VendorDetailsViewModel ViewModel
        {
            get { return (VendorDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(VendorDetailsViewModel), typeof(VendorDetails), new PropertyMetadata(null));
        #endregion

        public void SetFocus()
        {
            details.SetFocus();
        }

        private void Doc_Delete_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32( ((Button)sender).Tag.ToString());
           ViewModel.DeleteDocument(identity);
        }

        private void Doc_Dpwnload_Click(object sender, RoutedEventArgs e)
        {
            var identity = Convert.ToInt32(((Button)sender).Tag.ToString());
            ViewModel.DownloadDocument(identity);

        }

        private void accountNo_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(x => char.IsDigit(x)).ToArray());
            sender.SelectionStart = sender.Text.Length;
        }

        private void accountNo_TextChanging_1(TextBox sender, TextBoxTextChangingEventArgs args)
        {

        }
    }
}
