using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
    public class CheckListOfPropertyModel
    {
        public int CheckListPropertyId { get; set; }
        public int PropertyCheckListId { get; set; }
        public int CheckListId { get; set; }
        public bool Mandatory { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool Delete { get; set; }

        public ObservableCollection<PropertyCheckListDocumentsModel> Documents { get; set; }
    }
}
