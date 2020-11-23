using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Data
{
    [Table("Vendor")]
    public partial class Vendor
    {
        [Key]
        public int VendorId { get; set; }
        public Guid VendorGuid { get; set; }
        public string VendorSalutation { get; set; }
        public string VendorLastName { get; set; }
        public string VendorName { get; set; }
        public string VendorAlias { get; set; }
        public string RelativeSalutation { get; set; }
        public string RelativeName { get; set; }
        public string RelativeLastName { get; set; }
        public string ContactPerson { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string PhoneNoIsdCode { get; set; }
        public string PhoneNo { get; set; }
        public string email { get; set; }
        public string PAN { get; set; }
        public string AadharNo { get; set; }
        public string GSTIN { get; set; }
        public bool IsVendorActive { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }

        public virtual ICollection<VendorDocument> VendorDocuments { get; set; }
    }
}
