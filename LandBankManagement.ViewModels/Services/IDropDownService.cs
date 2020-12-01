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
    }
}
