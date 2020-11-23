using LandBankManagement.Models;
using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class CompanyCard : UserControl
    {
        public CompanyCard()
        {
            this.InitializeComponent();
        }

        #region ViewModel
        public CompanyDetailsViewModel ViewModel
        {
            get { return (CompanyDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CompanyDetailsViewModel), typeof(CompaniesCard), new PropertyMetadata(null));
        #endregion

        #region Item
        public CompanyModel Item
        {
            get { return (CompanyModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(CompanyModel), typeof(CompaniesCard), new PropertyMetadata(null));
        #endregion
    }
}
