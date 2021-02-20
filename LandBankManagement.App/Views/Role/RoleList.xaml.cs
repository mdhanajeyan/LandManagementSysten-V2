using LandBankManagement.Common;
using LandBankManagement.Controls;
using LandBankManagement.Models;
using LandBankManagement.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Views
{
    public sealed partial class RoleList : UserControl
    {
        public RoleList()
        {
            this.InitializeComponent();
        }
        #region ViewModel
        public RoleListViewModel ViewModel
        {
            get { return (RoleListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(RoleListViewModel), typeof(RoleList), new PropertyMetadata(null));
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
            var model = (RoleModel)sfdataGrid.SelectedItem;
            if (model != null)
                ViewModel.OnSelectedRow(model);
        }
    }
}
