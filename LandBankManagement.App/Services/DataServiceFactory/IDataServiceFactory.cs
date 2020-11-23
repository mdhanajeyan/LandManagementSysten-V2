
using LandBankManagement.Data.Services;

namespace LandBankManagement.Services
{
    public interface IDataServiceFactory
    {
        IDataService CreateDataService();
    }
}
