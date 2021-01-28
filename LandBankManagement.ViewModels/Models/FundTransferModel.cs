using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class FundTransferModel : ObservableObject
    {
        static public FundTransferModel CreateEmpty() => new FundTransferModel { FundTransferId = -1, IsEmpty = true };

        public int FundTransferId { get; set; }
        public Guid FundTransferGuid { get; set; }
        public int PayeeId { get; set; }
        public int PayeePaymentType { get; set; }
        public int PayeeBankId { get; set; }
        public int PayeeCashId { get; set; }
        public DateTimeOffset DateOfPayment { get; set; }
        public string Amount { get; set; }
        public string Narration { get; set; }
        public int ReceiverId { get; set; }
        public int ReceiverPaymentType { get; set; }
        public int ReceiverBankId { get; set; }
        public int ReceiverCashId { get; set; }

        public string FromCompanyName { get; set; }
        public string ToCompanyName { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }

        public bool IsNew => FundTransferId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is FundTransferModel model)
            {
                Merge(model);
            }
        }

        public void Merge(FundTransferModel source)
        {
            if (source != null)
            {
                FundTransferId = source.FundTransferId;
                FundTransferGuid = source.FundTransferGuid;
                PayeeId = source.PayeeId;
                PayeePaymentType = source.PayeePaymentType;
                PayeeBankId = source.PayeeBankId;
                DateOfPayment = source.DateOfPayment;
                Amount = source.Amount;
                Narration = source.Narration;
                ReceiverId = source.ReceiverId;
                ReceiverPaymentType = source.ReceiverPaymentType;
                ReceiverBankId = source.ReceiverBankId;
            }
        }

    }
}
