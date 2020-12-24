using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class DealPayScheduleModel : ObservableObject
    {
        public int DealPayScheduleId { get; set; }
        public int DealId { get; set; }
        public DateTimeOffset ScheduleDate { get; set; }
        public string Description { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Total { get; set; }
        public int Identity { get; set; }
    }
}
