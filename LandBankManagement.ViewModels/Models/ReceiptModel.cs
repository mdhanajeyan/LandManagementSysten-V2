using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class ReceiptModel : ObservableObject
    {
        static public ReceiptModel CreateEmpty() => new ReceiptModel { ReceiptId = -1, IsEmpty = true };

        public int ReceiptId { get; set; }
        public Guid ReceiptGuid { get; set; }
        public int PayeeId { get; set; }
        public int DealId { get; set; }
        public int PartyId { get; set; }
        public int PaymentTypeId { get; set; }
        public int DepositBankId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }

        public bool IsNew => ReceiptId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is ReceiptModel model)
            {
                Merge(model);
            }
        }

        public void Merge(ReceiptModel source)
        {
            if (source != null)
            {
                ReceiptId = source.ReceiptId;
                ReceiptGuid = source.ReceiptGuid;
                PayeeId = source.PayeeId;
                DealId = source.DealId;
                PartyId = source.PartyId;
                PaymentTypeId = source.PaymentTypeId;
                DepositBankId = source.DepositBankId;
                DateOfPayment = source.DateOfPayment;
                Amount = source.Amount;
                Narration = source.Narration;
            }
        }
    }
}
