using LandBankManagement.Services;
using System;
using System.Collections.ObjectModel;


namespace LandBankManagement.Models
{
    public class PropertyPartyModel : ObservableObject
    {
        public int PropertyPartyId { get; set; }
        public Guid PropertyGuid { get; set; }
        public int PartyId { get; set; }
        public int PropertyId { get; set; }
        public string PartyName { get; set; }
        public bool? IsPrimaryParty { get; set; }
        public bool IsGroup { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is PropertyPartyModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PropertyPartyModel source)
        {
            if (source != null)
            {
                PropertyPartyId = source.PropertyPartyId;
                PropertyGuid = source.PropertyGuid;
                PartyId = source.PartyId;
                PropertyId = source.PropertyId;
                PartyName = source.PartyName;

            }
        }
       
    }
}
