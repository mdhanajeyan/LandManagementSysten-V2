using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Data
{
	public class PaymentList
    {
        public int PaymentListId { get; set; }
        public int PropertyId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string ChequeNo { get; set; }
        public string Narration { get; set; }
        public bool PDC { get; set; }
        public bool PaymentTypeId { get; set; }

	}
}
