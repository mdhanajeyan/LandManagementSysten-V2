using LandBankManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LandBankManagement.Services
{
    public interface IDropDownService
    {
        Task<ObservableCollection<ComboBoxOptions>> GetHobliOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetAllHobliOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetHobliOptionsByTaluk(int talukId);
        Task<ObservableCollection<ComboBoxOptions>> GetVillageOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetAllVillageOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetVillageOptionsByHobli(int hobliId);
        Task<ObservableCollection<ComboBoxOptions>> GetCompanyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetAllCompanyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetTalukOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetAllTalukOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetAccountTypeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetExpenseHeadOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPartyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetDocumentTypeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetAllDocumentTypeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPropertyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetUnSoldPropertyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetCashOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetCashOptionsByCompany(int companyId);
        Task<ObservableCollection<ComboBoxOptions>> GetBankOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetBankOptionsByCompany(int companyId);
        Task<ObservableCollection<ComboBoxOptions>> GetVendorOptions();
        ObservableCollection<ComboBoxOptions> GetReportingToOptions();
        ObservableCollection<ComboBoxOptions> GetGenderOptions();
        ObservableCollection<ComboBoxOptions> GetGroupsTypeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetGroupsOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPartyOptions(string party);
        Task<ObservableCollection<ComboBoxOptions>> GetPartyOptionsByProperty(int propertyId);
        Task<ObservableCollection<ComboBoxOptions>>  GetPartyOptionsByGroup(int groupId);
        Task<ObservableCollection<ComboBoxOptions>> GetPropertyTypeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetRoleOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetVendorOptions(string vendor);
        Task<ObservableCollection<ComboBoxOptions>> GetCheckListOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPropertyCheckListOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPropertyMergeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPropertyOptionsByCompanyID(int companyId);
        Task<ObservableCollection<ComboBoxOptions>> GetDealOptions();
        ObservableCollection<ComboBoxOptions> GetSalutationOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetDocumentTypesByPropertyID(int propertyId);
        Task<ObservableCollection<ComboBoxOptions>> GetDealPartiesOptions(int dealId);
        Task<ObservableCollection<ComboBoxOptions>> GetGroupsOptionsForParty();
    }
}
