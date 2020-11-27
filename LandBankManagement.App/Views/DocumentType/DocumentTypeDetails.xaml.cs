using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class DocumentTypeDetails : UserControl
    {
        public DocumentTypeDetails()
        {
            this.InitializeComponent();
        }
        public DocumentTypeDetailsViewModel ViewModel
        {
            get { return (DocumentTypeDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(DocumentTypeDetailsViewModel), typeof(DocumentTypeDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
