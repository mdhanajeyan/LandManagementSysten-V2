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
    public sealed partial class DealReportView : Page
    {
        public DealReportViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }
        private DealReportViewer DealReportViewer { get; set; }

        public DealReportView()
        {
            ViewModel = ServiceLocator.Current.GetService<DealReportViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();

            InitializeComponent();

            DealReportViewer = new DealReportViewer(reportViewer, ViewModel);

            Loaded += ReportParameters_Loaded;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadDeals();
            DealReportViewer.UpdateDataSet();
        }


        async void ReportParameters_Loaded(object sender, RoutedEventArgs e)
        {

            reportViewer.ReportLoaded += reportViewer_ReportLoaded;
            reportViewer.ViewButtonClick += reportViewer_ViewButtonClick;

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                if (DealReportViewer != null)
                {
                    DealReportViewer.LoadReport();
                }
            }));
        }

        void reportViewer_ViewButtonClick(object sender, CancelEventArgs args)
        {
            DealReportViewer.UpdateDataSet();
        }

        void reportViewer_ReportLoaded(object sender, EventArgs e)
        {
            DealReportViewer.SetParameter();          
            DealReportViewer.UpdateDataSet();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            reportViewer.ReportLoaded -= reportViewer_ReportLoaded;
            reportViewer.ViewButtonClick -= reportViewer_ViewButtonClick;
            Loaded -= ReportParameters_Loaded;

            if (reportViewer.DataSources != null)
            {
                foreach (var dataDataSource in reportViewer.DataSources)
                {
                    IList list = dataDataSource.Value as IList;

                    if (list != null)
                    {
                        list.Clear();
                    }
                }
                reportViewer.DataSources.Clear();
            }

            reportViewer.Reset();
            reportViewer.Dispose();
        }
    }
}
