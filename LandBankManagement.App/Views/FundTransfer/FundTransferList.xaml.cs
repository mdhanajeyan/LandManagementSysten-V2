using LandBankManagement.Common;
using LandBankManagement.Controls;
using LandBankManagement.Models;
using LandBankManagement.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class FundTransferList : UserControl
    {
        public FundTransferList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public FundTransferListViewModel ViewModel
        {
            get { return (FundTransferListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(FundTransferListViewModel), typeof(FundTransferList), new PropertyMetadata(null));
        #endregion

        private async void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Query = args.QueryText;
            await ViewModel.RefreshAsync();
            //  QuerySubmittedCommand?.TryExecute(args.QueryText);
        }
        private async void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {

            switch (e.ClickedButton)
            {
                case ToolbarButton.New:
                    break;
                case ToolbarButton.Delete:
                    break;
                case ToolbarButton.Select:
                    break;
                case ToolbarButton.Refresh:
                    await ViewModel.RefreshAsync();
                    break;
                case ToolbarButton.Cancel:
                    break;
            }
        }

        private void sfdataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            var model = (FundTransferModel)sfdataGrid.SelectedItem;
            if (model != null)
                ViewModel.OnSelectedRow(model);
        }
    }
}
