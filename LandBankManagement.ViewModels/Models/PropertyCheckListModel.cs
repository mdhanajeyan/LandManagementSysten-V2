using LandBankManagement.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyCheckListModel : ObservableObject
    {
        static public PropertyCheckListModel CreateEmpty() => new PropertyCheckListModel { PropertyCheckListId = 0, IsEmpty = true };
        public int PropertyCheckListId { get; set; }

        public string CompanyID { get; set; }
        public Guid PropertyGuid { get; set; }
        public string PropertyName { get; set; }
        public string TalukId { get; set; }
        public string HobliId { get; set; }
        public string VillageId { get; set; }
        public string DocumentTypeId { get; set; }
        public string PropertyTypeId { get; set; }
        public string SurveyNo { get; set; }
        public string PropertyGMapLink { get; set; }
        public string LandAreaInputAcres { get; set; }
        public string LandAreaInputGuntas { get; set; }
        public string LandAreaInputAanas { get; set; }
        public string LandAreaInAcres { get; set; }
        public string LandAreaInGuntas { get; set; }
        public string AKarabAreaInputAanas { get; set; }
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
        public string BKarabAreaInputAanas { get; set; }
        public string BKarabAreaInAcres { get; set; }

        public string BKarabAreaInGuntas { get; set; }
        public string BKarabAreaInSqMts { get; set; }
        public string BKarabAreaInSqft { get; set; }
        public bool CheckListMaster { get; set; }
        public string PropertyDescription { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string CompanyName { get; set; }
        public string VillageName { get; set; }
        public string TotalArea { get; set; }
        public bool IsNew => PropertyCheckListId <= 0;
        public ObservableCollection<PropertyCheckListDocumentsModel> PropertyCheckListDocuments { get; set; }
        public ObservableCollection<CheckListOfPropertyModel> CheckListOfProperties { get; set; }
        public ObservableCollection<PropertyCheckListVendorModel> PropertyCheckListVendors { get; set; }

        public ObservableCollection<ComboBoxOptions> StatusOption { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is PropertyModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PropertyCheckListModel source)
        {
            if (source != null)
            {
                PropertyCheckListId = source.PropertyCheckListId;
                PropertyGuid = source.PropertyGuid;
                PropertyName = source.PropertyName;
                TalukId = source.TalukId;
                HobliId = source.HobliId;
                VillageId = source.VillageId;
                DocumentTypeId = source.DocumentTypeId;
                PropertyTypeId = source.PropertyTypeId;
                SurveyNo = source.SurveyNo;
                PropertyGMapLink = source.PropertyGMapLink;
                LandAreaInputAcres = source.LandAreaInputAcres;
                LandAreaInputGuntas = source.LandAreaInputGuntas;
                LandAreaInputAanas = source.LandAreaInputAanas;
                LandAreaInAcres = source.LandAreaInAcres;
                LandAreaInGuntas = source.LandAreaInGuntas;
                LandAreaInSqMts = source.LandAreaInSqMts;
                LandAreaInSqft = source.LandAreaInSqft;
                AKarabAreaInputAcres = source.AKarabAreaInputAcres;
                AKarabAreaInputGuntas = source.AKarabAreaInputGuntas;
                AKarabAreaInputAanas = source.AKarabAreaInputAanas;
                AKarabAreaInAcres = source.AKarabAreaInAcres;
                AKarabAreaInGuntas = source.AKarabAreaInGuntas;
                AKarabAreaInSqMts = source.AKarabAreaInSqMts;
                AKarabAreaInSqft = source.AKarabAreaInSqft;
                BKarabAreaInputAcres = source.BKarabAreaInputAcres;
                BKarabAreaInputGuntas = source.BKarabAreaInputGuntas;
                BKarabAreaInputAanas = source.BKarabAreaInputAanas;
                BKarabAreaInAcres = source.BKarabAreaInAcres;
                BKarabAreaInGuntas = source.BKarabAreaInGuntas;
                BKarabAreaInSqMts = source.BKarabAreaInSqMts;
                BKarabAreaInSqft = source.BKarabAreaInSqft;
                PropertyCheckListDocuments = source.PropertyCheckListDocuments;
                CheckListOfProperties = source.CheckListOfProperties;
                PropertyCheckListVendors = source.PropertyCheckListVendors;
                CompanyName = source.CompanyName;
                VillageName = source.VillageName;
                Remarks = source.Remarks;
                Status = source.Status;
                TotalArea = source.TotalArea;
            }
        }

    }
}
