using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.Models;
using LandBankManagement.ViewModels;

namespace LandBankManagement.Views
{
    public sealed partial class CompaniesCard : UserControl
    {
        public CompaniesCard()
        {
            InitializeComponent();
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
