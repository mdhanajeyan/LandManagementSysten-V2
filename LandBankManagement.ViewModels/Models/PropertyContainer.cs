using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
   public class PropertyContainer
    {
        public ObservableCollection<PropertyPartyModel> PartyList { get; set; }
        public ObservableCollection<ImagePickerResult> DocList { get; set; }
        public ObservableCollection<PropertyModel> PropertyList { get; set; }
        public PropertyModel Item { get; set; }
    }
}
