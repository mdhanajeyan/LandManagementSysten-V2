﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using LandBankManagement.ViewModels;
using LandBankManagement.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LandBankManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PropertyCheckListView : Page
    {
        public PropertyCheckListViewModel ViewModel { get; set; }

        public INavigationService NavigationService { get; }
        public PropertyCheckListView()
        {
            ViewModel = ServiceLocator.Current.GetService<PropertyCheckListViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
          
            ViewModel.PropertyCheckListDetials.IsEditMode = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            ViewModel.LoadAsync(e.Parameter as PropertyCheckListListArgs);
          
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            //await NavigationService.CreateNewViewAsync<PartiesViewModel>(ViewModel.PartyList.CreateArgs());
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {
            //ViewModel.PartyDetails.CancelEdit();

            //await NavigationService.CreateNewViewAsync<VendorDetailsViewModel>(ViewModel.PartyDetails.CreateArgs());

        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }

        bool isFirstTimeLoading = true;
        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ((Pivot)sender).SelectedIndex;
            if (index == 0)
            {if (isFirstTimeLoading)
                {
                    isFirstTimeLoading = false;
                    return;
                }
                await ViewModel.ViewModelList.RefreshAsync();
            }
        }
    }
}
