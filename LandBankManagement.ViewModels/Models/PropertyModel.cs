using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyModel : ObservableObject
    {
        static public PropertyModel CreateEmpty() => new PropertyModel { PropertyId = -1, IsEmpty = true };
        [Key]
        public int PropertyId { get; set; }
        public Guid PropertyGuid { get; set; }
        public string PropertyName { get; set; }
        public int PartyId { get; set; }
        public int TalukId { get; set; }
        public int HobliId { get; set; }
        public int VillageId { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime DateOfExecution { get; set; }
        public string DocumentNo { get; set; }
        public int PropertyTypeId { get; set; }
        public string SurveyNo { get; set; }
        public string PropertyGMapLink { get; set; }
        public decimal LandAreaInAcres { get; set; }
        public decimal LandAreaInGuntas { get; set; }
        public decimal LandAreaInSqMts { get; set; }
        public decimal LandAreaInSqft { get; set; }
        public decimal AKarabAreaInAcres { get; set; }
        public decimal AKarabAreaInGuntas { get; set; }
        public decimal AKarabAreaInSqMts { get; set; }
        public decimal AKarabAreaInSqft { get; set; }
        public decimal BKarabAreaInAcres { get; set; }
        public decimal BKarabAreaInGuntas { get; set; }
        public decimal BKarabAreaInSqMts { get; set; }
        public decimal BKarabAreaInSqft { get; set; }

        public bool IsNew => PropertyId <= 0;


        public override void Merge(ObservableObject source)
        {
            if (source is PropertyModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PropertyModel source)
        {
            if (source != null)
            {
                PropertyId = source.PropertyId;
                PropertyGuid = source.PropertyGuid;
                PropertyName = source.PropertyName;
                PartyId = source.PartyId;
                TalukId = source.TalukId;
                HobliId = source.HobliId;
                VillageId = source.VillageId;
                DocumentTypeId = source.DocumentTypeId;
                DateOfExecution = source.DateOfExecution;
                DocumentNo = source.DocumentNo;
                PropertyTypeId = source.PropertyTypeId;
                SurveyNo = source.SurveyNo;
                PropertyGMapLink = source.PropertyGMapLink;
                LandAreaInAcres = source.LandAreaInAcres;
                LandAreaInGuntas = source.LandAreaInGuntas;
                LandAreaInSqMts = source.LandAreaInSqMts;
                LandAreaInSqft = source.LandAreaInSqft;
                AKarabAreaInAcres = source.AKarabAreaInAcres;
                AKarabAreaInGuntas = source.AKarabAreaInGuntas;
                AKarabAreaInSqMts = source.AKarabAreaInSqMts;
                AKarabAreaInSqft = source.AKarabAreaInSqft;
                BKarabAreaInAcres = source.BKarabAreaInAcres;
                BKarabAreaInGuntas = source.BKarabAreaInGuntas;
                BKarabAreaInSqMts = source.BKarabAreaInSqMts;
                BKarabAreaInSqft = source.BKarabAreaInSqft;
            }
        }

    }
}
