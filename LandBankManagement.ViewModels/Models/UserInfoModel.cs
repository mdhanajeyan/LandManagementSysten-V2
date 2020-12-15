using LandBankManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class UserInfoModel : ObservableObject
    {
        public static UserInfoModel CreateEmpty() => new UserInfoModel { UserInfoId = -1, IsEmpty = true };
        public int UserInfoId { get; set; }
        public int UserRoleId { get; set; }
        public string UserName { get; set; }
        public string loginName { get; set; }
        public string UserPassword { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public List<RolePermission> Permission { get; set; }
        public object PictureSource { get; set; }
        public override void Merge(ObservableObject source)
        {
            if (source is UserInfoModel model)
            {
                Merge(model);
            }
        }

        public void Merge(UserInfoModel source)
        {
            if (source != null)
            {
                UserInfoId = source.UserInfoId;
                UserName = source.UserName;
                loginName = source.loginName;
                UserPassword = source.UserPassword;
                Email = source.Email;
                MobileNo = source.MobileNo;
                IsActive = source.IsActive;
                IsAdmin = source.IsAdmin;
                Created = source.Created;
                CreatedBy = source.CreatedBy;
                Updated = source.Updated;
                UpdatedBy = source.UpdatedBy;
                PictureSource = source.PictureSource;
            }
        }


    }
}
