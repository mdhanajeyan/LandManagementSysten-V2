using System;
using System.Threading.Tasks;

namespace LandBankManagement.Services
{
    public interface ILoginService
    {
        bool IsAuthenticated { get; set; }

        Task<bool> SignInWithPasswordAsync(string userName, string password);

        bool IsWindowsHelloEnabled(string userName);
        Task TrySetupWindowsHelloAsync(string userName);
        Task<Result> SignInWithWindowsHelloAsync();

        void Logoff();
    }
}
