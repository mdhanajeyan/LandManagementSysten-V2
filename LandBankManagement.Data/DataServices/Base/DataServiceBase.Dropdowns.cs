using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<Dictionary<int, string>> GetTalukOptions()
        {

            return await _dataSource.Taluks.Select(x => new { x.TalukId, x.TalukName }).ToDictionaryAsync(t => t.TalukId, t => t.TalukName);
        }
        public async Task<Dictionary<int, string>> GetHobliOptions()
        {
            return await _dataSource.Hoblis.Select(x => new { x.HobliId, x.HobliName }).ToDictionaryAsync(t => t.HobliId, t => t.HobliName);
        }
        public async Task<Dictionary<int, string>> GetVillageOptions()
        {
            return await _dataSource.Villages.Where(x=>x.VillageIsActive==true).Select(x => new { x.VillageId, x.VillageName }).ToDictionaryAsync(t => t.VillageId, t => t.VillageName);
        }
        public async Task<Dictionary<int, string>> GetCompanyOptions()
        {
            return await _dataSource.Companies.Where(x=>x.IsActive).Select(x => new { x.CompanyID, x.Name }).ToDictionaryAsync(t => t.CompanyID, t => t.Name);
        }
        public async Task<Dictionary<int, string>> GetAccountTypeOptions()
        {
            return await _dataSource.AccountTypes.Select(x => new { x.AccountTypeId, x.AccountTypeName }).ToDictionaryAsync(t => t.AccountTypeId, t => t.AccountTypeName);
        }
        public async Task<Dictionary<int, string>> GetExpenseHeadOptions()
        {
            return await _dataSource.ExpenseHeads.Where(x=>x.IsExpenseHeadActive==true).Select(x => new { x.ExpenseHeadId, x.ExpenseHeadName }).ToDictionaryAsync(t => t.ExpenseHeadId, t => t.ExpenseHeadName);
        }
        public async Task<Dictionary<int, string>> GetPartyOptions()
        {
            return await _dataSource.Parties.Where(x=>x.IsPartyActive==true).Select(x => new { x.PartyId, x.PartyFirstName }).ToDictionaryAsync(t => t.PartyId, t => t.PartyFirstName);
        }
        public async Task<Dictionary<int, string>> GetDocumentTypeOptions()
        {
            return await _dataSource.DocumentTypes.Select(x => new { x.DocumentTypeId, x.DocumentTypeName }).ToDictionaryAsync(t => t.DocumentTypeId, t => t.DocumentTypeName);
        }
        public async Task<Dictionary<int, string>> GetPropertyOptions()
        {
            return await _dataSource.Properties.Select(x => new { x.PropertyId, x.PropertyName }).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
        }

        public async Task<Dictionary<int, string>> GetUnSoldPropertyOptions()
        {
            return await _dataSource.Properties.Where(x=>x.IsSold==false || x.IsSold==null).Select(x => new { x.PropertyId, x.PropertyName }).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
        }

        public async Task<Dictionary<int, string>> GetCashOptions()
        {
            return await _dataSource.CashAccounts.Where(x=>x.IsCashAccountActive==true).Select(x => new { x.CashAccountId, x.CashAccountName }).ToDictionaryAsync(t => t.CashAccountId, t => t.CashAccountName);
        }
        public async Task<Dictionary<int, string>> GetBankOptions()
        {
            return await _dataSource.BankAccounts.Where(x=>x.IsBankAccountActive).Select(x => new { x.BankAccountId, x.BankName }).ToDictionaryAsync(t => t.BankAccountId, t => t.BankName);
        }
        public async Task<Dictionary<int, string>> GetVendorOptions()
        {
            return await _dataSource.Vendors.Where(x=>x.IsVendorActive==true).Select(x => new { x.VendorId, x.VendorName }).ToDictionaryAsync(t => t.VendorId, t => t.VendorName);
        }


        public Dictionary<int, string> GetReportingToOptions()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add(1, "Managers");
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

        public async  Task<Dictionary<int, string>> GetPartyOptions(string party)
        {
            return await _dataSource.Parties.Where(x=>x.PartyFirstName.Contains(party)).Select(x => new { x.PartyId, x.PartyFirstName }).ToDictionaryAsync(t => t.PartyId, t => t.PartyFirstName);
        }

        public async Task<Dictionary<int, string>> GetPropertyTypeOptions()
        {
            return await _dataSource.PropertyTypes.Select(x => new { x.PropertyTypeId, x.PropertyTypeText }).ToDictionaryAsync(t => t.PropertyTypeId, t => t.PropertyTypeText);
        }

        public async Task<Dictionary<int, string>> GetRoleOptions()
        {
            return await _dataSource.Roles.Select(x => new { x.RoleId, x.Name }).ToDictionaryAsync(t => t.RoleId, t => t.Name);
        }

        public async Task<Dictionary<int, string>> GetVendorOptions(string party)
        {
            return await _dataSource.Vendors.Where(x => x.VendorName.Contains(party)).Select(x => new { x.VendorId, x.VendorName }).ToDictionaryAsync(t => t.VendorId, t => t.VendorName);
        }

        public async Task<Dictionary<int, string>> GetCheckListOptions()
        {
            return await _dataSource.CheckLists.Select(x => new { x.CheckListId, x.CheckListName }).ToDictionaryAsync(t => t.CheckListId, t => t.CheckListName);
        }

        public async Task<Dictionary<int, string>> GetPropertyMergeOptions()
        {
            return await _dataSource.PropertyMerge.Where(x=>x.ForProposal==false).Select(x => new { x.PropertyMergeId, x.PropertyMergeDealName }).ToDictionaryAsync(t => t.PropertyMergeId, t => t.PropertyMergeDealName);
        }

        public async Task<Dictionary<int, string>> GetPropertyOptionsByCompanyID(int companyId)
        {
            if(companyId==0)
            return await _dataSource.Properties.Select(x => new { x.PropertyId, x.PropertyName }).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
            else
            return await _dataSource.Properties.Where(x=>x.CompanyID==companyId).Select(x => new { x.PropertyId, x.PropertyName }).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
        }

        public async Task<Dictionary<int, string>> GetDealOptions()
        {
            return await (from d in _dataSource.Deal
                        join pm in _dataSource.PropertyMerge on d.PropertyMergeId equals pm.PropertyMergeId
                        select new { d.DealId, pm.PropertyMergeDealName }).ToDictionaryAsync(t => t.DealId, t => t.PropertyMergeDealName);           
        }
    }
}
