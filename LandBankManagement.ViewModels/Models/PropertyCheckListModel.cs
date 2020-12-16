using LandBankManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyCheckListModel : ObservableObject
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public IEnumerable<PropertyCheckListModel> Children { get; set; }
    }
}
