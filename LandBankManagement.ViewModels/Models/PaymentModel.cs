using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
    public class PaymentModel : ObservableObject
    {
        static public PaymentModel CreateEmpty() => new PaymentModel { PaymentId = -1, IsEmpty = true };

        public int PaymentId { get; set; }
        public Guid PaymentGuid { get; set; }
        public string PayeeId { get; set; }
        public int PayeeTypeId { get; set; }
        public string ExpenseHeadId { get; set; }
        public string PropertyId { get; set; }
        public string PartyId { get; set; }
        public string DocumentTypeId { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTimeOffset DateOfPayment { get; set; }
        public string Amount { get; set; }
        public string ChequeNo { get; set; }
        public string Narration { get; set; }
        public string CashAccountId { get; set; }
        public string BankAccountId { get; set; }
        public bool PDC { get; set; }
        public ObservableCollection<PaymentListModel> PaymentListModel { get; set; }
        public string AccountName { get; set; }
        public bool IsNew => PaymentId <= 0;
        public string CompanyName { get; set; }
        public string PropertyName { get; set; }
        public string DocumentTypeName { get; set; }
        public string GroupId { get; set; }
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
                CashAccountId = source.CashAccountId;
                BankAccountId = source.BankAccountId;
                PDC = source.PDC;
                PaymentListModel = source.PaymentListModel;
            }
        }

    }
}
