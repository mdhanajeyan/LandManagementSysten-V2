using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LandBankManagement.Data
{
	public class Payment
    {
        public int PaymentId { get; set; }
        public Guid PaymentGuid { get; set; }
        public int PayeeId { get; set; }
        public int PayeeTypeId { get; set; }
        public int ExpenseHeadId { get; set; }
        public int PropertyId { get; set; }
        public int PartyId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string ChequeNo { get; set; }
        public string Narration { get; set; }


        [NotMapped]
        public string SearchTerms { get; set; }
    }
}
