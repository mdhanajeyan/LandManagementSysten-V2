using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class UserModel : ObservableObject
    {
        public static UserModel CreateEmpty() => new UserModel { UserId = -1, IsEmpty = true };
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string loginName { get; set; }
        public string UserPassword { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAgent { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is UserModel model)
            {
                Merge(model);
            }
        }

        public void Merge(UserModel source)
        {
            if (source != null)
            {
                UserId = source.UserId;
                UserName = source.UserName ;
                loginName = source.loginName;
                UserPassword = source.UserPassword;
                Code = source.Code;
                Email = source.Email;
                MobileNo = source.MobileNo;
                FromDate = source.FromDate;
                ToDate = source.ToDate;
                IsActive = source.IsActive;
                IsAgent = source.IsAgent;
                IsAdmin = source.IsAdmin;
                Created = source.Created;
                CreatedBy = source.CreatedBy;
                Updated = source.Updated;
                UpdatedBy = source.UpdatedBy;
            }
        }


    }
}
