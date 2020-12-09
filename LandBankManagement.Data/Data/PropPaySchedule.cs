using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Data.Data
{
    public class PropPaySchedule
    {
        public int PropPayScheduleId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Description { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
    }
}
