using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LandBankManagement.ViewModels
{
    public class PropertyCheckListArgs
    {
        static public PropertyTypeListArgs CreateEmpty() => new PropertyTypeListArgs { IsEmpty = true };

        public PropertyCheckListArgs()
        {
            OrderBy = r => r.Name;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.PropertyCheckList, object>> OrderBy { get; set; }
        public Expression<Func<Data.PropertyCheckList, object>> OrderByDesc { get; set; }
    }
    public class PropertyCheckListViewModel : ViewModelBase
    {
        public PropertyCheckListListViewModel ViewModelList { get; set; }
        public PropertyCheckListViewModel(ICommonServices commonServices) : base(commonServices)
        {
            
        }

        public void LoadAsync()
        {
            ViewModelList.LoadData();
        }
    }
}
