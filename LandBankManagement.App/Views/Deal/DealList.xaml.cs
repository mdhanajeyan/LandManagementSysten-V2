using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LandBankManagement.ViewModels;
using System;
namespace LandBankManagement.Views
{
    public sealed partial class DealList : UserControl
    {
        public DealList()
        {
            this.InitializeComponent();
        }

        #region ViewModel
        public DealListViewModel ViewModel
        {
            get { return (DealListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(DealListViewModel), typeof(DealList), new PropertyMetadata(null));
        #endregion

       
    }
}
