using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class PaymentScheduleModel
    {
        public int PropertyId { get; set; }
        public int ScheduleId{get;set;}
        public DateTimeOffset ScheduleDate { get; set; }
        public string Description { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Total { get; set; }
    }
}
