using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
   public class PropertyMergeListModel : ObservableObject
    {
        public int PropertyMergeListId { get; set; }
        public Guid PropertyMergeGuid { get; set; }
        public Guid PropertyGuid { get; set; }
        public string PropertyName { get; set; }
        public string Party { get; set; }
        public string Village { get; set; }
        public string SurveyNo { get; set; }
        public string LandArea { get; set; }
        public string AKarab { get; set; }
        public string BKarab { get; set; }
        public string SaleValue1 { get; set; }
        public string SaleValue2 { get; set; }
        public string Amount1 { get; set; }
        public string Amount2 { get; set; }        
    }
}
