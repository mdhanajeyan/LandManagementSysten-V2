using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Data 
{
    public class PropertyType
    {
        public int PropertyTypeId { get; set; }
        public Guid PropertyTypeGuid { get; set; }
        public string PropertyTypeText { get; set; }
        public bool PropertyTypeIsActive { get; set; }
	}
}
