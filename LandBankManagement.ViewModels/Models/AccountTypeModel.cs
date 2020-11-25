namespace LandBankManagement.Models
{
    public class AccountTypeModel : ObservableObject
    {
        static public AccountTypeModel CreateEmpty() => new AccountTypeModel { AccountTypeId = -1, IsEmpty = true };

        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }

        public bool IsNew => AccountTypeId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is AccountTypeModel model)
            {
                Merge(model);
            }
        }

        public void Merge(AccountTypeModel source)
        {
            if (source != null)
            {
                AccountTypeId = source.AccountTypeId;
                AccountTypeName = source.AccountTypeName;
            }
        }

    }
}
