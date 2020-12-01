using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class CompanyReportArgs
    {
        static public CompanyReportArgs CreateEmpty() => new CompanyReportArgs { IsEmpty = true };

        public CompanyReportArgs()
        {
            OrderBy = r => r.Name;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Company, object>> OrderBy { get; set; }
        public Expression<Func<Company, object>> OrderByDesc { get; set; }
    }
    public class CompanyReportViewModel
    {
        public ICompanyService CompanyService { get; }
        public CompanyReportArgs ViewModelArgs { get; private set; }
        public CompanyReportViewModel(ICompanyService companyService)
        {
            CompanyService = companyService;
        }

        public List<CompanyModel> ReportItems { get; set; }



        public async Task LoadCompanies()
        {
            ViewModelArgs = new CompanyReportArgs();
            IList<CompanyModel> result = await CompanyService.GetCompaniesAsync();
            ReportItems = (List<CompanyModel>)result;
        }


    }
}
