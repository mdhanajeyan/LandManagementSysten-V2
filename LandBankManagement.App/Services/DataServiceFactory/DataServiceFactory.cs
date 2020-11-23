using System;

using LandBankManagement.Data.Services;

namespace LandBankManagement.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        static private Random _random = new Random(0);

        public IDataService CreateDataService()
        {
            if (AppSettings.Current.IsRandomErrorsEnabled)
            {
                if (_random.Next(20) == 0)
                {
                    throw new InvalidOperationException("Random error simulation");
                }
            }

            return new SQLServerDataService(AppSettings.Current.SQLServerConnectionString);
        }
    }
}
