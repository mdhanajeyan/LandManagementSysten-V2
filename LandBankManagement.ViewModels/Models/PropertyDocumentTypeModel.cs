using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyDocumentTypeModel: ObservableObject
    {
        public int PropertyDocumentTypeId { get; set; }
        public int PropertyId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public string LandAreaInputAcres { get; set; }
        public string LandAreaInputGuntas { get; set; }
        public string LandAreaInputAanas { get; set; }
        public string LandAreaInAcres { get; set; }
        public string LandAreaInGuntas { get; set; }
        public string LandAreaInSqMts { get; set; }
        public string LandAreaInSqft { get; set; }
        public string AKarabAreaInputAcres { get; set; }
        public string AKarabAreaInputGuntas { get; set; }
        public string AKarabAreaInputAanas { get; set; }
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
        public decimal SaleValue1 { get; set; }
        public decimal SaleValue2 { get; set; }
        public ObservableCollection<ImagePickerResult> PropertyDocuments { get; set; }
    }
}
