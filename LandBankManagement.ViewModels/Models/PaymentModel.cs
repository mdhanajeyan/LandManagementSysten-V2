using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class PaymentModel : ObservableObject
    {
        static public PaymentModel CreateEmpty() => new PaymentModel { PaymentId = -1, IsEmpty = true };

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

        public bool IsNew => PaymentId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is PaymentModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PaymentModel source)
        {
            if (source != null)
            {
                PaymentId = source.PaymentId;
                PaymentGuid = source.PaymentGuid;
                PayeeId = source.PayeeId;
                PayeeTypeId = source.PayeeTypeId;
                ExpenseHeadId = source.ExpenseHeadId;
                PropertyId = source.PropertyId;
                PartyId = source.PartyId;
                DocumentTypeId = source.DocumentTypeId;
                PaymentTypeId = source.PaymentTypeId;
                DateOfPayment = source.DateOfPayment;
                Amount = source.Amount;
                ChequeNo = source.ChequeNo;
                Narration = source.Narration;
            }
        }

    }
}
