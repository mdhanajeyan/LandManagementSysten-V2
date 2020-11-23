using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;


namespace LandBankManagement.Services
{
    public class CompanyCollection : VirtualCollection<CompanyModel>
    {
        private DataRequest<Company> _dataRequest = null;

        public CompanyCollection(ICompanyService companyService, ILogService logService) : base(logService)
        {
            CompanyService = companyService;
        }

        public ICompanyService CompanyService { get; }

        private CompanyModel _defaultItem = CompanyModel.CreateEmpty();
        protected override CompanyModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Company> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await CompanyService.GetCompaniesCountAsync(_dataRequest);
                Ranges[0] = await CompanyService.GetCompaniesAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<CompanyModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await CompanyService.GetCompaniesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("CompanyCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
