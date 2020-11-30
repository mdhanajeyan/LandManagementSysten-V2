using LandBankManagement.Services;
using LandBankManagement.ViewModels;
using Syncfusion.UI.Xaml.Reports;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LandBankManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CompanyReportView : Page, IDisposable
    {
        public CompanyViewModel ViewModel { get; }
        public ReportViewerHelper Helper { get; set; }
        public INavigationService NavigationService { get; }
        public CompanyReportView()
        {
            ViewModel = ServiceLocator.Current.GetService<CompanyViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = new CompanyListArgs();
            await ViewModel.CompanyList.LoadAsync(param);
            SetReportData();
        }
        private void SetReportData()
        {
            Helper = new CompanyReportViewer(reportViewer, ViewModel);

            this.Loaded += ReportParamLoaded;
        }

        async void ReportParamLoaded(object sender, RoutedEventArgs e)
        {
            this.reportViewer.ReportLoaded += reportViewer_ReportLoaded;
            this.reportViewer.ViewButtonClick += reportViewer_ViewButtonClick;

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                if (this.Helper != null)
                {
                    this.Helper.LoadReport();
                }
            }));
        }

        void reportViewer_ViewButtonClick(object sender, CancelEventArgs args)
        {
            this.Helper.UpdateDataSet();
        }

        void reportViewer_ReportLoaded(object sender, EventArgs e)
        {
            this.Helper.SetParameter();
            this.Helper.UpdateDataSet();
        }

        public void Dispose()
        {
            Helper = null;
            this.reportViewer.ReportLoaded -= reportViewer_ReportLoaded;
            this.reportViewer.ViewButtonClick -= reportViewer_ViewButtonClick;
            this.Loaded -= ReportParamLoaded;

            if (this.reportViewer.DataSources != null)
            {
                foreach (var dataDataSource in this.reportViewer.DataSources)
                {
                    IList list = dataDataSource.Value as IList;

                    if (list != null)
                    {
                        list.Clear();
                    }
                }
                this.reportViewer.DataSources.Clear();
            }

            this.reportViewer.Reset();
            this.reportViewer.Dispose();
        }
    }
    public class CompanyReportViewer : ReportViewerHelper
    {
        private CompanyViewModel ViewModel { get; set; }
        public CompanyReportViewer(SfReportViewer reportViewerControl, CompanyViewModel companyViewModel)
        {
            this.ReportViewer = reportViewerControl;
            ViewModel = companyViewModel;
            this.FolderPath = "Company";
            this.ReportName = "CompanyReport";
        }

        public override void UpdateDataSet()
        {
            this.ReportViewer.DataSources.Clear();
            this.ReportViewer.DataSources.Add(new ReportDataSource { Name = "ProductCatalog", Value = ViewModel.CompanyList.Items });
        }
    }

    #region ReportViewerHelper
    public abstract class ReportViewerHelper
    {
        #region IReportViewerHelper Members

        public virtual void SetParameter() { }

        public virtual void UpdateDataSet() { }

        public virtual void LoadReport()
        {
            try
            {
                Assembly assembly = typeof(CompanyReportView).GetTypeInfo().Assembly;
                Stream reportStream = assembly.GetManifestResourceStream("LandBankManagement.App.Reports." + this.FolderPath + "." + this.ReportName + ".rdlc");
                this.ReportViewer.ProcessingMode = ProcessingMode.Local;

                this.ReportViewer.LoadReport(reportStream);
                this.ReportViewer.RefreshReport();
            }
            catch
            { }
        }

        public virtual Stream GetReportStream()
        {
            Assembly assembly = typeof(CompanyReportView).GetTypeInfo().Assembly;
            Stream reportStream = assembly.GetManifestResourceStream("LandBankManagement.App.Reports." + this.FolderPath + "." + this.ReportName + ".rdlc");
            return reportStream;
        }

        #endregion

        #region IReportViewerHelper Members

        public SfReportViewer ReportViewer
        {
            get;
            set;
        }

        public string ReportName
        {
            get;
            set;
        }

        public string FolderPath
        {
            get;
            set;
        }

        #endregion
    }

    #endregion
}
