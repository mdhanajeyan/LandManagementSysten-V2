using System;
using LandBankManagement.Services;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(ICommonServices commonServices) : base(commonServices)
        {

        }

        public Task LoadAsync()
        {
            throw new MissingMethodException();
        }

        public void Unload()
        {
        }

        public void ItemSelected(string item)
        {
        }
    }
}
