using System;
using LandBankManagement.Services;
using System.Threading.Tasks;
using LandBankManagement.Models;
using System.Collections.Generic;
using LandBankManagement.Data;

namespace LandBankManagement.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(ICompanyService companyService, ICommonServices commonServices) : base(commonServices)
        {
            CompanyService = companyService;
        }

        public ICompanyService CompanyService { get; }

        private IList<CompanyModel> _companies = null;
        public IList<CompanyModel> Companies
        {
            get => _companies;
            set => Set(ref _companies, value);
        }
        public async Task LoadAsync()
        {
            StartStatusMessage("Loading dashboard...");
            await LoadCompaniesAsync();

            EndStatusMessage("Dashboard loaded");
        }
        public void Unload()
        {
            Companies = null;

        }

        private async Task LoadCompaniesAsync()
        {
            try
            {
                var request = new DataRequest<Company>
                {
                    OrderByDesc = r => r.Name
                };
                Companies = await CompanyService.GetCompaniesAsync(request);
            }
            catch (Exception ex)
            {
                LogException("Dashboard", "Load Companies", ex);
            }
        }
        public void ItemSelected(string item)
        {
            switch (item)
            {
                case "Companies":
                    NavigationService.Navigate<CompanyViewModel>(new CompanyListArgs { OrderByDesc = r => r.Name });
                    break;
                default:
                    break;
            }
        }
    }
}
