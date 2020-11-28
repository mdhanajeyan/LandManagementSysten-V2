using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class CheckListList : UserControl
    {
        public CheckListList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public CheckListListViewModel ViewModel
        {
            get { return (CheckListListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CheckListListViewModel), typeof(CheckListList), new PropertyMetadata(null));
        #endregion
    }
}
