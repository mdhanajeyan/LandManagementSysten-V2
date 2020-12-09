using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IronSnappy;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class DropDownService:IDropDownService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public DropDownService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }
        public ObservableCollection<ComboBoxOptions> GetHobliOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetHobliOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetTalukOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetTalukOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }
        public ObservableCollection<ComboBoxOptions> GetVillageOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetVillageOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }

        }
        public ObservableCollection<ComboBoxOptions> GetCompanyOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetCompanyOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetAccountTypeOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetAccountTypeOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetExpenseHeadOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetExpenseHeadOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }
        public ObservableCollection<ComboBoxOptions> GetPartyOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetPartyOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetDocumentTypeOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetDocumentTypeOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetPropertyOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetPropertyOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetCashOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetCashOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetBankOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetBankOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetVendorOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetVendorOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }
        public ObservableCollection<ComboBoxOptions> GetReportingToOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetReportingToOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }
        public ObservableCollection<ComboBoxOptions> GetGenderOptions()
        {
            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetGenderOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }

        public ObservableCollection<ComboBoxOptions> GetPartyOptions(string party) {

            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetPartyOptions(party);
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                return list;
            }
        }
        public ObservableCollection<ComboBoxOptions> GetPropertyTypeOptions()
        {

            ObservableCollection<ComboBoxOptions> list = new ObservableCollection<ComboBoxOptions>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var models = dataService.GetPropertyTypeOptions();
                foreach (var obj in models)
                {
                    list.Add(new ComboBoxOptions
                    {
                        Id = obj.Key,
                        Description = obj.Value
                    });
                }
                list.Insert(0, new ComboBoxOptions { Id = 0, Description = "" });
                return list;
            }
        }
    }
}
