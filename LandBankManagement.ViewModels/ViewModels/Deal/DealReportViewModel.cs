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
    public class DealReportArgs
    {
        static public DealReportArgs CreateEmpty() => new DealReportArgs { IsEmpty = true };

        public DealReportArgs()
        {
            OrderBy = r => r.DealId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Deal, object>> OrderBy { get; set; }
        public Expression<Func<Deal, object>> OrderByDesc { get; set; }
    }
    public class DealReportViewModel
    {
        public IDealService DealService { get; }
        public DealReportArgs ViewModelArgs { get; private set; }
        public string Query { get; set; }
        public DealReportViewModel(IDealService dealService)
        {
            DealService = dealService;
        }

        public List<DealModel> ReportItems { get; set; }



        public async Task LoadDeals()
        {
            ViewModelArgs = new DealReportArgs();
            DataRequest<Data.Deal> request = BuildDataRequest();
            IList<DealModel> result = await DealService.GetDealsAsync(request);
            ReportItems = new List<DealModel>();
            foreach (var obj in result) {
                ReportItems.Add(obj);
            }
           // ReportItems = result.ToList();
        }
        private DataRequest<Deal> BuildDataRequest()
        {
            return new DataRequest<Deal>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

    }
}
