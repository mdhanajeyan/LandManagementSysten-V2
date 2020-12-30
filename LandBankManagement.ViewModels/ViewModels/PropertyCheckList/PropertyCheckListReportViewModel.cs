using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class PropertyCheckListReportArgs
    {
        static public PropertyCheckListReportArgs CreateEmpty() => new PropertyCheckListReportArgs { IsEmpty = true };

        public PropertyCheckListReportArgs()
        {
            OrderBy = r => r.PropertyCheckListId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<PropertyCheckList, object>> OrderBy { get; set; }
        public Expression<Func<PropertyCheckList, object>> OrderByDesc { get; set; }
    }
    public class PropertyCheckListReportViewModel
    {
        public IPropertyCheckListService PropertyCheckListService { get; }
        public PropertyCheckListReportArgs ViewModelArgs { get; private set; }
        public string Query { get; set; }
        public PropertyCheckListReportViewModel(IPropertyCheckListService propertyCheckListService)
        {
            PropertyCheckListService = propertyCheckListService;
        }

        public List<PropertyCheckListModel> ReportItems { get; set; }



        public async Task LoadPropertyCheckLists()
        {
            ViewModelArgs = new PropertyCheckListReportArgs();
            DataRequest<Data.PropertyCheckList> request = BuildDataRequest();
            IList<PropertyCheckListModel> result = await PropertyCheckListService.GetPropertyCheckListAsync(request);
            ReportItems = new List<PropertyCheckListModel>();
            foreach (var obj in result)
            {
                ReportItems.Add(obj);
            }
            // ReportItems = result.ToList();
        }
        private DataRequest<PropertyCheckList> BuildDataRequest()
        {
            return new DataRequest<PropertyCheckList>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }
    }
}
