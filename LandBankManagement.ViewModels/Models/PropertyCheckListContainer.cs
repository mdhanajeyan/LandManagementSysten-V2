using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyCheckListContainer
    {
        public PropertyCheckListModel Item { get; set; }
        public ObservableCollection<CheckListOfPropertyModel> CheckList { get; set; }
        public ObservableCollection<PropertyCheckListVendorModel> VendorList { get; set; }
    }
}
