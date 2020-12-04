using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
	public class UserRoleModel : ObservableObject
    {
        public static UserRoleModel CreateEmpty() => new UserRoleModel { UserRoleId = -1, IsEmpty = true };
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

        public bool IsNew => UserRoleId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is UserRoleModel model)
            {
                Merge(model);
            }
        }

        public void Merge(UserRoleModel source)
        {
            if (source != null)
            {
                UserRoleId = source.UserRoleId;
                UserId = source.UserId;
                RoleId = source.RoleId;
                Created = source.Created;
                CreatedBy = source.CreatedBy;
                Updated = source.Updated;
                UpdatedBy = source.UpdatedBy;
            }
        }
    }
}
