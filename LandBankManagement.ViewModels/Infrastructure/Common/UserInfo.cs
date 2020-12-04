using System;

namespace LandBankManagement
{
    public class UserInfo
    {
        static readonly public UserInfo Default = new UserInfo
        {
            AccountName = "Madhu Sudhan",
            FirstName = "Madhu",
            LastName = "N"
        };

        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object PictureSource { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public bool IsEmpty => String.IsNullOrEmpty(DisplayName.Trim());
    }
}
