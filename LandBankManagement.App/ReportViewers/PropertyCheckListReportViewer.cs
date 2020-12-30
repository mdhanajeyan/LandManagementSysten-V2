using BoldReports.UI.Xaml;
using LandBankManagement.Infrastructure;
using LandBankManagement.Models;
using LandBankManagement.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LandBankManagement.ReportViewers
{
    public class PropertyCheckListReportViewer : ReportViewerHelper
    {
        private readonly PropertyCheckListReportViewModel ViewModel;
        public PropertyCheckListReportViewer(ReportViewer reportViewerControl, PropertyCheckListReportViewModel viewModel)
        {
            ReportViewer = reportViewerControl;
            ReportName = "PropertyCheckListReport";
            ViewModel = viewModel;
        }

        public override void UpdateDataSet()
        {
            var models = ViewModel.ReportItems;
            if (models == null)
            {
                models = new List<PropertyCheckListModel>();

            }
            ReportViewer.DataSources.Clear();
            ReportViewer.DataSources.Add(new ReportDataSource { Name = "DataSet1", Value = models });
        }

    }
}
