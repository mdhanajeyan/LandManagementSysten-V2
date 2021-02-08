using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Data
{
    public class PropertyCheckListVendor
    {
        [Key]
        public int CheckListVendorId { get; set; }
        public int VendorId { get; set; }
        public int PropertyCheckListId { get; set; }
        public bool? IsPrimaryVendor { get; set; }
        [NotMapped]
        public string VendorName { get; set; }
    }
}
