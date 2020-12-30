using BoldReports.UI.Xaml;
using LandBankManagement.Infrastructure;
using LandBankManagement.Models;
using LandBankManagement.ViewModels;
using System.Collections.Generic;

namespace LandBankManagement.ReportViewers
{
   public class DealReportViewer : ReportViewerHelper
    {
        private readonly DealReportViewModel ViewModel;
        public DealReportViewer(ReportViewer reportViewerControl, DealReportViewModel viewModel)
        {
            ReportViewer = reportViewerControl;
            ReportName = "DealReport";
            ViewModel = viewModel;
        }

        public override void UpdateDataSet()
        {
            var DealModels = ViewModel.ReportItems;
            if (DealModels == null)
            {
                DealModels = new List<DealModel>();
            }
            ReportViewer.DataSources.Clear();
            ReportViewer.DataSources.Add(new ReportDataSource { Name = "DataSet1", Value = DealModels });
        }
    }
}
