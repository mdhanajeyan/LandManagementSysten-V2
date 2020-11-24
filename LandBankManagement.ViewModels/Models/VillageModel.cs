using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class VillageModel : ObservableObject
    {
        static public VillageModel CreateEmpty() => new VillageModel { VillageId = -1, IsEmpty = true };
        public int VillageId { get; set; }
        public Guid VillageGuid { get; set; }
        public int TalukId { get; set; }
        public int HobliId { get; set; }
        public string VillageName { get; set; }
        public string VillageGMapLink { get; set; }
        public bool VillageIsActive { get; set; }
        public bool IsNew => VillageId <= 0;
        public override void Merge(ObservableObject source)
        {
            if (source is VillageModel model)
            {
                Merge(model);
            }
        }

        public void Merge(VillageModel source)
        {
            if (source != null)
            {
                VillageId = source.VillageId;
                VillageGuid = source.VillageGuid;
                TalukId = source.TalukId;
                HobliId = source.HobliId;
                VillageName = source.VillageName;
                VillageGMapLink = source.VillageGMapLink;
                VillageIsActive = source.VillageIsActive;
            }
        }

    }
}
