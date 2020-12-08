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
        public Dictionary<int, string> GetExpenseHeadOptions()
        {
            return _dataSource.ExpenseHeads.Select(x => new { x.ExpenseHeadId, x.ExpenseHeadName }).ToDictionary(t => t.ExpenseHeadId, t => t.ExpenseHeadName);
        }
        public Dictionary<int, string> GetPartyOptions()
        {
            return _dataSource.Parties.Select(x => new { x.PartyId, x.PartyFirstName }).ToDictionary(t => t.PartyId, t => t.PartyFirstName);
        }
        public Dictionary<int, string> GetDocumentTypeOptions()
        {
            return _dataSource.DocumentTypes.Select(x => new { x.DocumentTypeId, x.DocumentTypeName }).ToDictionary(t => t.DocumentTypeId, t => t.DocumentTypeName);
        }
        public Dictionary<int, string> GetPropertyOptions()
        {
            return _dataSource.Properties.Select(x => new { x.PropertyId, x.PropertyName }).ToDictionary(t => t.PropertyId, t => t.PropertyName);
        }
        public Dictionary<int, string> GetCashOptions()
        {
            return _dataSource.CashAccounts.Select(x => new { x.CashAccountId, x.CashAccountName }).ToDictionary(t => t.CashAccountId, t => t.CashAccountName);
        }
        public Dictionary<int, string> GetBankOptions()
        {
            return _dataSource.BankAccounts.Select(x => new { x.BankAccountId, x.BankName }).ToDictionary(t => t.BankAccountId, t => t.BankName);
        }
        public Dictionary<int, string> GetVendorOptions()
        {
            return _dataSource.Vendors.Select(x => new { x.VendorId, x.VendorName }).ToDictionary(t => t.VendorId, t => t.VendorName);
        }

        public Dictionary<int, string> GetReportingToOptions()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add( 1,"Managers");
            list.Add(2, "Groups");
            return list;
        }
        public Dictionary<int, string> GetGenderOptions()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add(1, "Male");
            list.Add(2, "Female");
            return list;
        }

        public Dictionary<int, string> GetPartyOptions(string party)
        {
            return _dataSource.Parties.Where(x=>x.PartyFirstName.Contains(party)).Select(x => new { x.PartyId, x.PartyFirstName }).ToDictionary(t => t.PartyId, t => t.PartyFirstName);
        }
    }
}
