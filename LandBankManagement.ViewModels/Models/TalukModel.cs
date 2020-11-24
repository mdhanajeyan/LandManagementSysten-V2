using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class TalukModel : ObservableObject
    {
        static public TalukModel CreateEmpty() => new TalukModel { TalukId = -1, IsEmpty = true };

        public int TalukId { get; set; }
        public Guid TalukGuid { get; set; }
        public string TalukName { get; set; }
        public string TalukGMapLink { get; set; }
        public bool TalukIsActive { get; set; }

        public bool IsNew => TalukId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is TalukModel model)
            {
                Merge(model);
            }
        }

        public void Merge(TalukModel source)
        {
            if (source != null)
            {
                TalukId = source.TalukId;
                TalukGuid = source.TalukGuid;
                TalukName = source.TalukName;
                TalukGMapLink = source.TalukGMapLink;
                TalukIsActive = source.TalukIsActive;
            }
        }
    }
}
