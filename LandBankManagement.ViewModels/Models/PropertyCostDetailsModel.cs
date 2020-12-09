using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class PropertyCostDetailsModel : ObservableObject
    {
        public int PropertyId { get; set; }
        public string ComapnyName { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string Taluk { get; set; }
        public string Hobli { get; set; }
        public string Village { get; set; }
        public string SurveyNo { get; set; }
        public string DocumentType { get; set; }
        public string SaleValue1 { get; set; }
        public string SaleValue2 { get; set; }
        public List<PropertyPartyModel> Parties {get;set;}
        public List<PaymentScheduleModel> PropPaySchedules { get; set; }
        public string Total { get; set; }
        public bool IsNew => true;
    }
}
