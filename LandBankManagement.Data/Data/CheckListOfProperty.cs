using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Data
{
    public class CheckListOfProperty
    {
        [Key]
        public int CheckListPropertyId { get; set; }
        public int PropertyCheckListId { get; set; }
        public int CheckListId { get; set; }
        public bool Mandatory { get; set; }      
        
        [NotMapped]
        public bool Delete { get; set; }
    }
}
