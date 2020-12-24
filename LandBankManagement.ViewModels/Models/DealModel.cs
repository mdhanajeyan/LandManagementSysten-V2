using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
    public class DealModel : ObservableObject
    {
        static public DealModel CreateEmpty() => new DealModel { DealId = -1, IsEmpty = true };

        public int DealId { get; set; }
        public int PropertyMergeId { get; set; }
        public int CompanyId { get; set; }
        public decimal SaleValue1 { get; set; }
        public decimal SaleValue2 { get; set; }

        public ObservableCollection<DealPartiesModel> DealParties = new ObservableCollection<DealPartiesModel>();
        public ObservableCollection<DealPayScheduleModel> DealPaySchedules =new ObservableCollection<DealPayScheduleModel>();
        public string DealName { get; set; }
        public string Amount1 { get; set; }
        public string Amount2 { get; set; }
        public decimal Total { get; set; }
        public bool IsNew => DealId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is DealModel model)
            {
                Merge(model);
            }
        }

        public void Merge(DealModel source)
        {
            if (source != null)
            {
                DealId = source.DealId;
                PropertyMergeId = source.PropertyMergeId;
                CompanyId = source.CompanyId;
                SaleValue1 = source.SaleValue1;
                SaleValue2 = source.SaleValue2;
                DealName = source.DealName;
                Amount1 = source.Amount1;
                Amount2 = source.Amount2;
               
            }
        }
    }
}
