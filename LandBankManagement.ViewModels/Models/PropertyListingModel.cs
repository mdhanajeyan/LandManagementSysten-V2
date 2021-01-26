using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LandBankManagement.Models
{
   public class PropertyListingModel : ObservableObject
    {
        public int id { get; set; }
        public string GroupName { get; set; }
        public Guid GroupGuid { get; set; }
        public bool ShowChildren { get; set; }
        public bool HideChildren { get; set; }
        public IEnumerable<PropertyModel> Children { get; set; }
    }
}
