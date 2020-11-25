using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
	public class CheckListModel : ObservableObject
    {
        static public CheckListModel CreateEmpty() => new CheckListModel { CheckListId = -1, IsEmpty = true };

        public int CheckListId { get; set; }
        public Guid CheckListGuid { get; set; }
        public string CheckListName { get; set; }
        public bool CheckListIsActive { get; set; }

        public bool IsNew => CheckListId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is CheckListModel model)
            {
                Merge(model);
            }
        }

        public void Merge(CheckListModel source)
        {
            if (source != null)
            {
                CheckListId = source.CheckListId;
                CheckListGuid = source.CheckListGuid;
                CheckListName = source.CheckListName;
                CheckListIsActive = source.CheckListIsActive;
            }
        }

    }
}
