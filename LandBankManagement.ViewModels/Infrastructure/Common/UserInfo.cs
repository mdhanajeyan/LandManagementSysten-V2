using System;

namespace LandBankManagement
{
    public class UserInfo
    {
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object PictureSource { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public bool IsEmpty => String.IsNullOrEmpty(DisplayName.Trim());
    }
}
