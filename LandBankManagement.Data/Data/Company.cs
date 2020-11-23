using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Data
{
    public class Company
    {
        public int CompanyID { get; set; }
        public Guid CompanyGuid { get; set; }
        public string Name { get; set; }
        public string PhoneNoIsdCode { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string GSTIN { get; set; }
        public bool IsActive { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }

        public virtual ICollection<CompanyDocument> CompanyDocuments { get; set; }

        public string BuildSearchTerms() => $"{CompanyID} {Name} {Email} {AddressLine1}".ToLower();
    }
}
