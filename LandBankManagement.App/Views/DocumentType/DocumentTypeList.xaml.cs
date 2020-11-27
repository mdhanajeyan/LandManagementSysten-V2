using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class DocumentTypeList : UserControl
    {
        public DocumentTypeList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public DocumentTypeListViewModel ViewModel
        {
            get { return (DocumentTypeListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(DocumentTypeListViewModel), typeof(DocumentTypeList), new PropertyMetadata(null));
        #endregion
    }
}
