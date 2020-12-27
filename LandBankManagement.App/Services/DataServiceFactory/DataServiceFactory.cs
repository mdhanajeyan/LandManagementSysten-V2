using System;
using System.Reflection;
using LandBankManagement.Data.Services;

namespace LandBankManagement.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        private readonly ILogService _logService;
        public DataServiceFactory(ILogService logService)
        {
            _logService = logService;
        }

        public IDataService CreateDataService()
        {
            var connectionString = AppSettings.Current.SQLServerConnectionString;
            _logService.WriteAsync(Data.LogType.Information, GetType().Name, MethodBase.GetCurrentMethod().Name, "Connection String", connectionString);
            return new SQLServerDataService(connectionString);
        }
    }
}
