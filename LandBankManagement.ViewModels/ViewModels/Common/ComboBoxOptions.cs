using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class ComboBoxOptions : ObservableObject
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
    }

    public class ComboBoxOptionsStringId : ObservableObject
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
    }
}
