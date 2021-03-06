﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class ReceiptModel : ObservableObject
    {
        static public ReceiptModel CreateEmpty() => new ReceiptModel { ReceiptId = -1, IsEmpty = true };

        public int ReceiptId { get; set; }
        public Guid ReceiptGuid { get; set; }
        public string PayeeId { get; set; }
        public string DealId { get; set; }
        public string PartyId { get; set; }
        public int PaymentTypeId { get; set; }
        public string DepositBankId { get; set; }
        public string DepositCashId { get; set; }
        public DateTimeOffset DateOfPayment { get; set; }
        public string Amount { get; set; }
        public string Narration { get; set; }
        public string BankName { get; set; }
        public string CashName { get; set; }
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
                BankName = source.BankName;
                CashName = source.CashName;
            }
        }
    }
}
