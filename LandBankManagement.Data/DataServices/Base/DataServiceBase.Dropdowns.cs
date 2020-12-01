using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public Dictionary<int, string> GetTalukOptions()
        {

            return _dataSource.Taluks.Select(x => new { x.TalukId, x.TalukName }).ToDictionary(t => t.TalukId, t => t.TalukName);
        }
        public Dictionary<int, string> GetHobliOptions()
        {
            return _dataSource.Hoblis.Select(x => new { x.HobliId, x.HobliName }).ToDictionary(t => t.HobliId, t => t.HobliName);
        }
        public Dictionary<int, string> GetVillageOptions()
        {
            return _dataSource.Villages.Select(x => new { x.VillageId, x.VillageName }).ToDictionary(t => t.VillageId, t => t.VillageName);
        }
        public Dictionary<int, string> GetCompanyOptions()
        {
            return _dataSource.Companies.Select(x => new { x.CompanyID, x.Name }).ToDictionary(t => t.CompanyID, t => t.Name);
        }
        public Dictionary<int, string> GetAccountTypeOptions()
        {
            return _dataSource.AccountTypes.Select(x => new { x.AccountTypeId, x.AccountTypeName }).ToDictionary(t => t.AccountTypeId, t => t.AccountTypeName);
        }
    }
}
