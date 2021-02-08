using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
   public class GroupsModel : ObservableObject
    {
        static public GroupsModel CreateEmpty() => new GroupsModel { GroupId = -1, IsEmpty = true };
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
        public string GroupType { get; set; }
        public string GroupTypeName { get; set; }
        public bool IsNew => GroupId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is FundTransferModel model)
            {
                Merge(model);
            }
        }

        public void Merge(GroupsModel source)
        {
            if (source != null)
            {
                GroupId = source.GroupId;
                GroupName = source.GroupName;
                GroupType = source.GroupType;
                IsActive = source.IsActive;
                GroupTypeName = source.GroupTypeName;
            }
        }
    }
}
