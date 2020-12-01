using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class HobliModel : ObservableObject
    {
        static public HobliModel CreateEmpty() => new HobliModel { HobliId = 0,TalukId=0 ,HobliIsActive=true};
        public int HobliId { get; set; }
        public Guid HobliGuid { get; set; }
        public int TalukId { get; set; }
        public string HobliName { get; set; }
        public string HobliGMapLink { get; set; }
        public bool HobliIsActive { get; set; }

        public string TalukName { get; set; }
        public bool IsNew => HobliId <= 0;
        public override void Merge(ObservableObject source)
        {
            if (source is HobliModel model)
            {
                Merge(model);
            }
        }

        public void Merge(HobliModel source)
        {
            if (source != null)
            {
                HobliId = source.HobliId;
                HobliGuid = source.HobliGuid;
                TalukId = source.TalukId;
                HobliId = source.HobliId;
                HobliName = source.HobliName;
                HobliGMapLink = source.HobliGMapLink;
                HobliIsActive = source.HobliIsActive;
            }
        }
    }
}
