
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

namespace LandBankManagement.Views
{
    public sealed partial class CompaniesList : UserControl
    {
        public CompaniesList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public CompanyListViewModel ViewModel
        {
            get { return (CompanyListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CompanyListViewModel), typeof(CompaniesList), new PropertyMetadata(null));
        #endregion
    }
}
