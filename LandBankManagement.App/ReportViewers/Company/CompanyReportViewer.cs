using BoldReports.UI.Xaml;
using LandBankManagement.Infrastructure;
using LandBankManagement.ViewModels;

namespace LandBankManagement.ReportViewers
{
    public class CompanyReportViewer : ReportViewerHelper
    {
        private readonly CompanyReportViewModel ViewModel;
        public CompanyReportViewer(ReportViewer reportViewerControl, CompanyReportViewModel viewModel)
        {
            ReportViewer = reportViewerControl;
            ReportName = "CompanyReport";
            ViewModel = viewModel;
        }

        public override void UpdateDataSet()
        {
            var companyModels = ViewModel.ReportItems;
            ReportViewer.DataSources.Clear();
            ReportViewer.DataSources.Add(new ReportDataSource { Name = "DataSet1", Value = companyModels });
        }
    }
}
