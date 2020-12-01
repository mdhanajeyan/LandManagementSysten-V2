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
    }
}
