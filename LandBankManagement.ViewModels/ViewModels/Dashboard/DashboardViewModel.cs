using LandBankManagement.Services;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(ICommonServices commonServices) : base(commonServices)
        {

        }

        public async Task LoadAsync()
        {
        }

        public void Unload()
        {
        }

        public void ItemSelected(string item)
        {
        }
    }
}
