using LandBankManagement.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LandBankManagement.Models
{
    public class PropertyModel : ObservableObject
    {
        static public PropertyModel CreateEmpty() => new PropertyModel { PropertyId = 0, IsEmpty = true };
        public int PropertyId { get; set; }
        public int CompanyID { get; set; }
        public Guid PropertyGuid { get; set; }
        public string PropertyName { get; set; }
        public int PartyId { get; set; }
        public int TalukId { get; set; }
        public int HobliId { get; set; }
        public int VillageId { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTimeOffset DateOfExecution { get; set; }
        public string DocumentNo { get; set; }
        public int PropertyTypeId { get; set; }
        public string SurveyNo { get; set; }
        public string PropertyGMapLink { get; set; }
        public string LandAreaInputAcres { get; set; }
        public string LandAreaInputGuntas { get; set; }
        public string LandAreaInAcres { get; set; }
        public string LandAreaInGuntas { get; set; }
        public string LandAreaInSqMts { get; set; }
        public string LandAreaInSqft { get; set; }
        public string AKarabAreaInputAcres { get; set; }
        public string AKarabAreaInputGuntas { get; set; }
        public string AKarabAreaInAcres { get; set; }
        public string AKarabAreaInGuntas { get; set; }
        public string AKarabAreaInSqMts { get; set; }
        public string AKarabAreaInSqft { get; set; }
        public string BKarabAreaInputAcres { get; set; }
        public string BKarabAreaInputGuntas { get; set; }
        public string BKarabAreaInAcres { get; set; }
        public string BKarabAreaInGuntas { get; set; }
        public string BKarabAreaInSqMts { get; set; }
        public string BKarabAreaInSqft { get; set; }
        public decimal SaleValue1 { get; set; }
        public decimal SaleValue2 { get; set; }
        public bool IsNew => PropertyId <= 0;

        public ObservableCollection<ImagePickerResult> PropertyDocuments { get; set; }
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
                LandAreaInputAcres = source.LandAreaInputAcres;
                LandAreaInputGuntas = source.LandAreaInputGuntas;
                LandAreaInAcres = source.LandAreaInAcres;
                LandAreaInGuntas = source.LandAreaInGuntas;
                LandAreaInSqMts = source.LandAreaInSqMts;
                LandAreaInSqft = source.LandAreaInSqft;
                AKarabAreaInputAcres = source.AKarabAreaInputAcres;
                AKarabAreaInputGuntas = source.AKarabAreaInputGuntas;
                AKarabAreaInAcres = source.AKarabAreaInAcres;
                AKarabAreaInGuntas = source.AKarabAreaInGuntas;
                AKarabAreaInSqMts = source.AKarabAreaInSqMts;
                AKarabAreaInSqft = source.AKarabAreaInSqft;
                BKarabAreaInputAcres = source.BKarabAreaInputAcres;
                BKarabAreaInputGuntas = source.BKarabAreaInputGuntas;
                BKarabAreaInAcres = source.BKarabAreaInAcres;
                BKarabAreaInGuntas = source.BKarabAreaInGuntas;
                BKarabAreaInSqMts = source.BKarabAreaInSqMts;
                BKarabAreaInSqft = source.BKarabAreaInSqft;
                SaleValue1 = source.SaleValue1;
                SaleValue2 = source.SaleValue2;
            }
        }

    }
}
