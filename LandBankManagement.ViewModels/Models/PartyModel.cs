using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
   public class PartyModel : ObservableObject
    {
        static public PartyModel CreateEmpty() => new PartyModel { PartyId = -1, IsEmpty = true };
        public int PartyId { get; set; }
        public Guid PartyGuid { get; set; }
        public string PartySalutation { get; set; }
        public string PartyLastName { get; set; }
        public string PartyName { get; set; }
        public string PartyAlias { get; set; }
        public string RelativeSalutation { get; set; }
        public string RelativeFirstName { get; set; }
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
        public bool IsPartyActive { get; set; }
        public string GSTIN { get; set; }
        public ObservableCollection<ImagePickerResult> partyDocuments { get; set; }
        public bool IsNew => PartyId <= 0;


        public override void Merge(ObservableObject source)
        {
            if (source is PartyModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PartyModel source)
        {
            if (source != null)
            {
                PartyId = source.PartyId;
                PartyName = source.PartyName;
                PartyGuid = source.PartyGuid;
                PartyAlias = source.PartyAlias;
                PartySalutation = source.PartySalutation;
                AadharNo = source.AadharNo;
                ContactPerson = source.ContactPerson;
                PAN = source.PAN;
                GSTIN = source.GSTIN;
                email = source.email;
                IsPartyActive = source.IsPartyActive;
                PhoneNo = source.PhoneNo;
                AddressLine1 = source.AddressLine1;
                AddressLine2 = source.AddressLine2;
                City = source.City;
                PinCode = source.PinCode;
                partyDocuments = source.partyDocuments;
            }
        }

        public override string ToString()
        {
            return IsEmpty ? "Empty" : PartyName;
        }
    }


}
