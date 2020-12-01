using Syncfusion.UI.Xaml.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LandBankManagement.Infrastructure
{
    public abstract class ReportViewerHelper
    {
        #region Helper Members

        public virtual void SetParameter() { }

        public virtual void UpdateDataSet() { }

        public virtual void LoadReport()
        {
            try
            {
                Stream reportStream = GetReportStream();
                this.ReportViewer.ProcessingMode = ProcessingMode.Local;

                this.ReportViewer.LoadReport(reportStream);
                this.ReportViewer.RefreshReport();
            }
            catch
            { }
        }

        public virtual Stream GetReportStream()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(ReportName + ".rdlc"));
            Stream reportStream = assembly.GetManifestResourceStream(resourceName);
            return reportStream;
        }

        #endregion

        #region SfReportViewerHelper Members

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

        #endregion
    }
}
