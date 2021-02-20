using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyCheckListVendorModel
    {
        public int CheckListVendorId { get; set; }
        public int VendorId { get; set; }
        public int PropertyCheckListId { get; set; }
        public bool? IsPrimaryVendor { get; set; }
        public bool IsGroup { get; set; }
        public string VendorName { get; set; }
    }
}
