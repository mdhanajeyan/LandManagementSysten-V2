using System;

namespace LandBankManagement.Data.Services
{
    public class SQLiteDataService : DataServiceBase
    {
        public SQLiteDataService(string connectionString)
            : base(new SQLiteDb(connectionString))
        {
        }
    }
}
