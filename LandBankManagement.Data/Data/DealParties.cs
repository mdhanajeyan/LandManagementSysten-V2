using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Data
{
    public class DealParties
    {
        [Key]
        public int DealPartyId { get; set; }
        public int DealId { get; set; }
        public int PartyId { get; set; }
        [NotMapped]
        public string PartyName { get; set; }
    }
}
