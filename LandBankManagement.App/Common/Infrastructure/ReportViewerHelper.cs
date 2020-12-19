using BoldReports.UI.Xaml;
using System.IO;
using System.Linq;
using System.Reflection;

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

        #region Bold ReportViewerHelper Members

        public ReportViewer ReportViewer
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
