namespace LandBankManagement.Models
{
    public class RolePermissionModel : ObservableObject
    {
        static public RolePermissionModel CreateEmpty() => new RolePermissionModel { RolePermissionId = -1, IsEmpty = true };


        public int RolePermissionId { get; set; }
        public string RoleInfoId { get; set; }
        public int ScreenId { get; set; }
        public bool OptionId { get; set; }
        public bool CanView { get; set; }
        public string ScreenName { get; set; }

        public bool IsNew => RolePermissionId <= 0;
        public override void Merge(ObservableObject source)
        {
            if (source is RolePermissionModel model)
            {
                Merge(model);
            }
        }

        public void Merge(RolePermissionModel source)
        {
            if (source != null)
            {
                RolePermissionId = source.RolePermissionId;
                RoleInfoId = source.RoleInfoId;
                ScreenId = source.ScreenId;
                OptionId = source.OptionId;
                CanView = source.CanView;
                ScreenName = source.ScreenName;
            }
        }
    }
}