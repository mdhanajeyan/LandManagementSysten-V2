using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
	public class CashAccountModel : ObservableObject
    {
        static public CashAccountModel CreateEmpty() => new CashAccountModel { CashAccountId = -1, IsEmpty = true };

        public int CashAccountId { get; set; }
        public Guid CashAccountGuid { get; set; }
        public int AccountTypeId { get; set; }
        public string CashAccountName { get; set; }
        public bool IsCashAccountActive { get; set; }

        public bool IsNew => CashAccountId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is CashAccountModel model)
            {
                Merge(model);
            }
        }

        public void Merge(CashAccountModel source)
        {
            if (source != null)
            {
                CashAccountId = source.CashAccountId;
                CashAccountGuid = source.CashAccountGuid;
                AccountTypeId = source.AccountTypeId;
                CashAccountName = source.CashAccountName;
                IsCashAccountActive = source.IsCashAccountActive;
            }
        }

    }
}
