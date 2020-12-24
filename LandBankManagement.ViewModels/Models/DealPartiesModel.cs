using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
   public class DealPartiesModel : ObservableObject
    {
        public int DealPartyId { get; set; }
        public int DealId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
    }
}
