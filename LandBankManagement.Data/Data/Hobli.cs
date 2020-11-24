using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Data
{
    public class Hobli
    {
        public int HobliId { get; set; }
        public Guid HobliGuid { get; set; }
        public int TalukId { get; set; }
        public string HobliName { get; set; }
        public string HobliGMapLink { get; set; }
        public bool HobliIsActive { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{TalukId} {HobliName}".ToLower();
    }
}
