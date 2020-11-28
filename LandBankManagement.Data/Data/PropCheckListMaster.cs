using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LandBankManagement.Data
{
	public class PropCheckListMaster
    {
        [Key]
        public int PropCheckListMasterId { get; set; }
        public Guid PropCheckListMasterGuid { get; set; }
        public string PropCheckListMasterDescription { get; set; }
       

        [NotMapped]
        public string SearchTerms { get; set; }
    }
}
