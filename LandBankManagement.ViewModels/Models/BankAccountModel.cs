using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class BankAccountModel : ObservableObject
    {
        static public BankAccountModel CreateEmpty() => new BankAccountModel { BankAccountId = -1, IsEmpty = true };
        [Key]
        public int BankAccountId { get; set; }
        public Guid BankGuid { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public int AccountType { get; set; }
        public string IFSCCode { get; set; }
        public decimal OpeningBalance { get; set; }
        public bool IsBankAccountActive { get; set; }

        public bool IsNew => BankAccountId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is BankAccountModel model)
            {
                Merge(model);
            }
        }

        public void Merge(BankAccountModel source)
        {
            if (source != null)
            {
                BankAccountId = source.BankAccountId;
                BankGuid = source.BankGuid;
                BankName = source.BankName;
                BranchName = source.BranchName;
                AccountNumber = source.AccountNumber;
                AccountType = source.AccountType;
                IFSCCode = source.IFSCCode;
                OpeningBalance = source.OpeningBalance;
                IsBankAccountActive = source.IsBankAccountActive;
            }
        }
    }
}
