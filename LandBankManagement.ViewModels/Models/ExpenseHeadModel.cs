using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
   public class ExpenseHeadModel : ObservableObject
    {
        static public ExpenseHeadModel CreateEmpty() => new ExpenseHeadModel { ExpenseHeadId = -1, IsEmpty = true };
        public int ExpenseHeadId { get; set; }
        public Guid ExpenseHeadGuid { get; set; }
        public string ExpenseHeadName { get; set; }
        public bool IsExpenseHeadActive { get; set; }

        public bool IsNew => ExpenseHeadId <= 0;


        public override void Merge(ObservableObject source)
        {
            if (source is ExpenseHeadModel model)
            {
                Merge(model);
            }
        }

        public void Merge(ExpenseHeadModel source)
        {
            if (source != null)
            {
                ExpenseHeadGuid = source.ExpenseHeadGuid;
                ExpenseHeadId = source.ExpenseHeadId;
                ExpenseHeadName = source.ExpenseHeadName;
                IsExpenseHeadActive = source.IsExpenseHeadActive;               

            }
        }

        public override string ToString()
        {
            return IsEmpty ? "Empty" : ExpenseHeadName;
        }
    }
}
