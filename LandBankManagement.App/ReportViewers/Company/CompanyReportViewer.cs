using LandBankManagement.Infrastructure;
using LandBankManagement.Models;
using LandBankManagement.Services;
using LandBankManagement.ViewModels;
using Syncfusion.UI.Xaml.Reports;
using System.Collections.Generic;

namespace LandBankManagement.ReportViewers
{
    public class CompanyReportViewer : ReportViewerHelper
    {
        private readonly CompanyReportViewModel ViewModel;
        public CompanyReportViewer(SfReportViewer reportViewerControl, CompanyReportViewModel viewModel)
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
