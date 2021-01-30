using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyMergeModel : ObservableObject
    {
        static public PropertyMergeModel CreateEmpty() => new PropertyMergeModel { PropertyMergeId = -1, IsEmpty = true };

        public int PropertyMergeId { get; set; }
        public Guid PropertyMergeGuid { get; set; }
        public string PropertyMergeDealName { get; set; }
        public decimal MergedTotalArea { get; set; }
        public decimal MergedSaleValue1 { get; set; }
        public decimal MergedSaleValue2 { get; set; }
        public decimal MergedAmountPaid1 { get; set; }
        public decimal MergedAmountPaid2 { get; set; }
        public decimal MergedBalancePayable1 { get; set; }
        public decimal MergedBalancePayable2 { get; set; }
        public bool ForProposal { get; set; }
        public bool IsSold { get; set; }
        public string FormattedTotalArea { get; set; }
        public ObservableCollection<PropertyMergeListModel> propertyMergeLists { get; set; }

        public bool IsNew => PropertyMergeId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is PropertyMergeModel model)
            {
                Merge(model);
            }
        }

        public void Merge(PropertyMergeModel source)
        {
            if (source != null)
            {
                PropertyMergeId = source.PropertyMergeId;
                PropertyMergeGuid = source.PropertyMergeGuid;
                PropertyMergeDealName = source.PropertyMergeDealName;
                MergedTotalArea = source.MergedTotalArea;
                MergedSaleValue1 = source.MergedSaleValue1;
                MergedSaleValue2 = source.MergedSaleValue2;
                MergedAmountPaid1 = source.MergedAmountPaid1;
                MergedAmountPaid2 = source.MergedAmountPaid2;
                MergedBalancePayable1 = source.MergedBalancePayable1;
                MergedBalancePayable2 = source.MergedBalancePayable2;
                ForProposal = source.ForProposal;
            }
        }
    }
}
