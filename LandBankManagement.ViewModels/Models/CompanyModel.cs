using LandBankManagement.Services;
using System;
using System.Collections.ObjectModel;

namespace LandBankManagement.Models
{
    public class CompanyModel : ObservableObject
    {
        static public CompanyModel CreateEmpty() => new CompanyModel { CompanyID = -1, IsEmpty = true };

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
        public ObservableCollection<ImagePickerResult> CompanyDocuments { get;set;}
        public bool IsNew => CompanyID <= 0;

       

        public override void Merge(ObservableObject source)
        {
            if (source is CompanyModel model)
            {
                Merge(model);
            }
        }

        public void Merge(CompanyModel source)
        {
            if (source != null)
            {
                CompanyID = source.CompanyID;
                CompanyGuid = source.CompanyGuid;
                Name = source.Name;
                PhoneNoIsdCode = source.PhoneNoIsdCode;
                PhoneNo = source.PhoneNo;
                Email = source.Email;
                PAN = source.PAN;
                GSTIN = source.GSTIN;
                AddressLine1 = source.AddressLine1;
                AddressLine2 = source.AddressLine2;
                City = source.City;
                IsActive = source.IsActive;
                Pincode = source.Pincode;
                CompanyDocuments = source.CompanyDocuments;
            }
        }

        public override string ToString()
        {
            return IsEmpty ? "Empty" : Name;
        }
    }
}
