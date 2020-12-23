using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Data.Data
{
	public class PropertyMerge
    {
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
	}
}
