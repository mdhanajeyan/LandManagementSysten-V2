using LandBankManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Services
{
    public interface IDropDownService
    {
        ObservableCollection<ComboBoxOptions> GetHobliOptions();
        ObservableCollection<ComboBoxOptions> GetVillageOptions();
        ObservableCollection<ComboBoxOptions> GetCompanyOptions();
        ObservableCollection<ComboBoxOptions> GetTalukOptions();
        ObservableCollection<ComboBoxOptions> GetAccountTypeOptions();
        ObservableCollection<ComboBoxOptions> GetExpenseHeadOptions();
        ObservableCollection<ComboBoxOptions> GetPartyOptions();
        ObservableCollection<ComboBoxOptions> GetDocumentTypeOptions();
        ObservableCollection<ComboBoxOptions> GetPropertyOptions();
        ObservableCollection<ComboBoxOptions> GetCashOptions();
        ObservableCollection<ComboBoxOptions> GetBankOptions();
        ObservableCollection<ComboBoxOptions> GetVendorOptions();
        ObservableCollection<ComboBoxOptions> GetReportingToOptions();
        ObservableCollection<ComboBoxOptions> GetGenderOptions();
        ObservableCollection<ComboBoxOptions> GetPartyOptions(string party);
        ObservableCollection<ComboBoxOptions> GetPropertyTypeOptions();
        ObservableCollection<ComboBoxOptions> GetRoleOptions();
    }
}
