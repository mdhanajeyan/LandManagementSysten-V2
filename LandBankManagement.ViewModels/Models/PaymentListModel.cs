using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class PaymentListModel
    {
        public int PaymentListId { get; set; }
        public int PaymentId { get; set; }
        public DateTimeOffset DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string ChequeNo { get; set; }
        public string Narration { get; set; }
        public bool PDC { get; set; }
        public bool PaymentTypeId { get; set; }
        public int CashAccountId { get; set; }
        public int BankAccountId { get; set; }
        public string AccountName { get; set; }
        public int identity { get; set; }
    }
}
