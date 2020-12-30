using LandBankManagement.ReportViewers;
using LandBankManagement.Services;
using LandBankManagement.ViewModels;
using BoldReports.UI.Xaml;
using System;
using System.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace LandBankManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PropertyCheckListReportView : Page
    {
        public PropertyCheckListReportViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }
        private PropertyCheckListReportViewer PropertyCheckListReportViewer { get; set; }

        public PropertyCheckListReportView()
        {
            ViewModel = ServiceLocator.Current.GetService<PropertyCheckListReportViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();

            InitializeComponent();

            PropertyCheckListReportViewer = new PropertyCheckListReportViewer(propertyReportViewer, ViewModel);

            Loaded += ReportParameters_Loaded;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadPropertyCheckLists();
            PropertyCheckListReportViewer.UpdateDataSet();
        }


        async void ReportParameters_Loaded(object sender, RoutedEventArgs e)
        {
            propertyReportViewer.ReportLoaded += reportViewer_ReportLoaded;
            propertyReportViewer.ViewButtonClick += reportViewer_ViewButtonClick;

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                if (PropertyCheckListReportViewer != null)
                {
                    PropertyCheckListReportViewer.LoadReport();
                }
            }));
        }

        void reportViewer_ViewButtonClick(object sender, CancelEventArgs args)
        {
            PropertyCheckListReportViewer.UpdateDataSet();
        }

        void reportViewer_ReportLoaded(object sender, EventArgs e)
        {
            PropertyCheckListReportViewer.SetParameter();
            PropertyCheckListReportViewer.UpdateDataSet();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            propertyReportViewer.ReportLoaded -= reportViewer_ReportLoaded;
            propertyReportViewer.ViewButtonClick -= reportViewer_ViewButtonClick;
            Loaded -= ReportParameters_Loaded;

            if (propertyReportViewer.DataSources != null)
            {
                foreach (var dataDataSource in propertyReportViewer.DataSources)
                {
                    IList list = dataDataSource.Value as IList;

                    if (list != null)
                    {
                        list.Clear();
                    }
                }
                propertyReportViewer.DataSources.Clear();
            }

            propertyReportViewer.Reset();
            propertyReportViewer.Dispose();
        }
    }
}
