using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class RoleModel : ObservableObject
    {
        static public RoleModel CreateEmpty() => new RoleModel { RoleId = -1, IsEmpty = true };

        public int RoleId { get; set; }
        public string Name { get; set; }
        public string ReportingTo { get; set; }
        public bool IsOrganizationRole { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsNew => RoleId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is RoleModel model)
            {
                Merge(model);
            }
        }

        public void Merge(RoleModel source)
        {
            if (source != null)
            {
                RoleId = source.RoleId;
                Name = source.Name;
                ReportingTo = source.ReportingTo;
                IsOrganizationRole = source.IsOrganizationRole;
                Created = source.Created;
                CreatedBy = source.CreatedBy;
                Updated = source.Updated;
                UpdatedBy = source.UpdatedBy;

            }
        }
    }
}
