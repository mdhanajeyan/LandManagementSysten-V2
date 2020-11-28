using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LandBankManagement.Models;

namespace LandBankManagement.Models
{
	public class PropCheckListMasterModel : ObservableObject
    {
        static public PropCheckListMasterModel CreateEmpty() => new PropCheckListMasterModel { PropCheckListMasterId = -1, IsEmpty = true };

        public int PropCheckListMasterId { get; set; }
        public Guid PropCheckListMasterGuid { get; set; }
        public string PropCheckListMasterDescription { get; set; }


        public bool IsNew => PropCheckListMasterId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is PropCheckListMasterModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PropCheckListMasterModel source)
        {
            if (source != null)
            {
                PropCheckListMasterId = source.PropCheckListMasterId;
                PropCheckListMasterGuid = source.PropCheckListMasterGuid;
                PropCheckListMasterDescription = source.PropCheckListMasterDescription;
            }

        }
    }
}
