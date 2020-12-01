using System;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.ViewManagement;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Windows.Storage;
using LandBankManagement.Services;
using LandBankManagement.ViewModels;
using LandBankManagement.Views;

namespace LandBankManagement
{
    static public class Startup
    {
        static private readonly ServiceCollection _serviceCollection = new ServiceCollection();

        static public async Task ConfigureAsync()
        {
            AppCenter.Start("2d3dec86-1b95-4594-85d0-edfc3197d4ca",
                   typeof(Analytics), typeof(Crashes));

            Analytics.TrackEvent("AppStarted");

            ServiceLocator.Configure(_serviceCollection);

            ConfigureNavigation();

            await EnsureLogDbAsync();


            var logService = ServiceLocator.Current.GetService<ILogService>();
            await logService.WriteAsync(Data.LogType.Information, "Startup", "Configuration", "Application Start", $"Application started.");

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
            
        }

        private static void ConfigureNavigation()
        {
            NavigationService.Register<ShellViewModel, ShellView>();
            NavigationService.Register<MainShellViewModel, MainShellView>();

            NavigationService.Register<DashboardViewModel, DashboardView>();

            NavigationService.Register<CompanyViewModel, CompanyView>();
            NavigationService.Register<CompanyDetailsViewModel, CompanyView>();
            NavigationService.Register<CompanyReportViewModel, CompanyReportView>();

            NavigationService.Register<AppLogsViewModel, AppLogsView>();

            NavigationService.Register<SettingsViewModel, SettingsView>();

            NavigationService.Register<VendorViewModel, VendorView>();
            NavigationService.Register<VendorDetailsViewModel, VendorView>();

            NavigationService.Register<PartyViewModel, PartyView>();
            NavigationService.Register<PartyDetailsViewModel, PartyView>();
            NavigationService.Register<ExpenseHeadViewModel, ExpenseHeadView>();
            NavigationService.Register<TalukViewModel, TalukView>();
            NavigationService.Register<HobliViewModel, HobliView>();
            NavigationService.Register<VillageViewModel, VillageView>();
            NavigationService.Register<CashAccountViewModel, CashAccountView>();
            NavigationService.Register<DocumentTypeViewModel, DocumentTypeView>();
            NavigationService.Register<CheckListViewModel, CheckListView>();
            NavigationService.Register<PropertyTypeViewModel, PropertyTypeView>();
            NavigationService.Register<BankAccountViewModel, BankAccountView>();
        }
       
        static private async Task EnsureLogDbAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var appLogFolder = await localFolder.CreateFolderAsync(AppSettings.AppLogPath, CreationCollisionOption.OpenIfExists);
            if (await appLogFolder.TryGetItemAsync(AppSettings.AppLogName) == null)
            {
                var sourceLogFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AppLog/AppLog.db"));
                var targetLogFile = await appLogFolder.CreateFileAsync(AppSettings.AppLogName, CreationCollisionOption.ReplaceExisting);
                await sourceLogFile.CopyAndReplaceAsync(targetLogFile);
            }
        }
    }
}
