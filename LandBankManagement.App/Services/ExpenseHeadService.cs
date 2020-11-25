using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class ExpenseHeadService: IExpenseHeadService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public ExpenseHeadService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddExpenseHeadAsync(ExpenseHeadModel model)
        {
            long id = model.ExpenseHeadId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var expense = new ExpenseHead();
                if (expense != null)
                {
                    UpdateExpenseHeadFromModel(expense, model);
                    expense.ExpenseHeadGuid = Guid.NewGuid();
                    await dataService.AddExpenseHeadAsync(expense);
                    model.Merge(await GetExpenseHeadAsync(dataService, expense.ExpenseHeadId));
                }
                return 0;
            }
        }

        public async Task<int> AddPartyAsync(ExpenseHeadModel model)
        {
            long id = model.ExpenseHeadId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var expense = new ExpenseHead();
                if (expense != null)
                {
                    UpdateExpenseHeadFromModel(expense, model);
                    expense.ExpenseHeadGuid = Guid.NewGuid();
                    await dataService.AddExpenseHeadAsync(expense);
                    model.Merge(await GetExpenseHeadAsync(dataService, expense.ExpenseHeadId));
                }
                return 0;
            }
        }

        public async Task<ExpenseHeadModel> GetExpenseHeadAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetExpenseHeadAsync(dataService, id);
            }
        }
        static private async Task<ExpenseHeadModel> GetExpenseHeadAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetExpenseHeadAsync(id);
            if (item != null)
            {
                return await CreateExpenseHeadModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<ExpenseHeadModel>> GetExpenseHeadsAsync(DataRequest<ExpenseHead> request)
        {
            var collection = new ExpenseHeadCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<ExpenseHeadModel>> GetExpenseHeadsAsync(int skip, int take, DataRequest<ExpenseHead> request)
        {
            var models = new List<ExpenseHeadModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetExpenseHeadsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateExpenseHeadModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetExpenseHeadsCountAsync(DataRequest<ExpenseHead> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetExpenseHeadsCountAsync(request);
            }
        }

        public async Task<int> UpdateExpenseHeadAsync(ExpenseHeadModel model)
        {
            long id = model.ExpenseHeadId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var expense = id > 0 ? await dataService.GetExpenseHeadAsync(model.ExpenseHeadId) : new ExpenseHead();
                if (expense != null)
                {
                    UpdateExpenseHeadFromModel(expense, model);
                    await dataService.UpdateExpenseHeadAsync(expense);
                    model.Merge(await GetExpenseHeadAsync(dataService, expense.ExpenseHeadId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteExpenseHeadAsync(ExpenseHeadModel model)
        {
            var expense = new ExpenseHead { ExpenseHeadId = model.ExpenseHeadId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteExpenseHeadAsync(expense);
            }
        }

        //public async Task<int> DeletepartyRangeAsync(int index, int length, DataRequest<Party> request)
        //{
        //    using (var dataService = DataServiceFactory.CreateDataService())
        //    {
        //        var items = await dataService.GetCompanyKeysAsync(index, length, request);
        //        return await dataService.DeleteCompanyAsync(items.ToArray());
        //    }
        //}

        static public async Task<ExpenseHeadModel> CreateExpenseHeadModelAsync(ExpenseHead source, bool includeAllFields)
        {
            var model = new ExpenseHeadModel()
            {
            ExpenseHeadId = source.ExpenseHeadId,
            ExpenseHeadGuid = source.ExpenseHeadGuid,
            ExpenseHeadName = source.ExpenseHeadName,
            IsExpenseHeadActive = source.IsExpenseHeadActive
        };
            return model;
        }

        private void UpdateExpenseHeadFromModel(ExpenseHead target, ExpenseHeadModel source)
        {
            target.ExpenseHeadId = source.ExpenseHeadId;
            target.ExpenseHeadGuid = source.ExpenseHeadGuid;
            target.ExpenseHeadName = source.ExpenseHeadName;
            target.IsExpenseHeadActive = source.IsExpenseHeadActive;
           
        }
    }
}
