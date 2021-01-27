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
        Task<ObservableCollection<ComboBoxOptions>> GetVillageOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetCompanyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetTalukOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetAccountTypeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetExpenseHeadOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPartyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetDocumentTypeOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPropertyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetUnSoldPropertyOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetCashOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetBankOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetVendorOptions();
        ObservableCollection<ComboBoxOptions> GetReportingToOptions();
        ObservableCollection<ComboBoxOptions> GetGenderOptions();
        Task<ObservableCollection<ComboBoxOptions>> GetPartyOptions(string party);
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
    }
}
