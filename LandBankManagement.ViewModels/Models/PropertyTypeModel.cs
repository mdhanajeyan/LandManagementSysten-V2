using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyTypeModel : ObservableObject
    {
        static public PropertyTypeModel CreateEmpty() => new PropertyTypeModel { PropertyTypeId = -1, IsEmpty = true };
        [Key]
        public int PropertyTypeId { get; set; }
        public Guid PropertyTypeGuid { get; set; }
        public string PropertyTypeText { get; set; }
        public bool PropertyTypeIsActive { get; set; }

        public bool IsNew => PropertyTypeId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is PropertyTypeModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PropertyTypeModel source)
        {
            if (source != null)
            {
                PropertyTypeId = source.PropertyTypeId;
                PropertyTypeGuid = source.PropertyTypeGuid;
                PropertyTypeText = source.PropertyTypeText;
                PropertyTypeIsActive = source.PropertyTypeIsActive;
            }
        }
    }
}
