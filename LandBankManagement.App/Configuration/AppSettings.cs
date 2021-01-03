using LandBankManagement.Models;
using LandBankManagement.Services;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.Storage;

namespace LandBankManagement
{
    public class AppSettings
    {
        static AppSettings()
        {
            Current = new AppSettings();
        }

        public static AppSettings Current { get; }

        public static readonly string AppLogPath = "AppLog";
        public static readonly string AppLogName = $"AppLog.1.0.db";
        public static readonly string AppLogFileName = Path.Combine(AppLogPath, AppLogName);

        public static readonly string DatabasePatternFileName = "";
        public static readonly string DatabasePath = "";
        public static readonly string DatabasePattern = "";
        public static readonly string DatabaseName = "";

        public readonly string AppLogConnectionString = $"Data Source={AppLogFileName}";

        private const string REGPATH = @"SOFTWARE\LeanSys\EminentLMS\Data";

        public readonly string DbVersion = $"1.0.0";
        public DataProviderType DataProvider = DataProviderType.SQLServer;

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;
        public ApplicationDataCompositeValue CompositeSettings => new ApplicationDataCompositeValue();

        public string Version
        {
            get
            {
                var ver = Package.Current.Id.Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
            }
        }

        public string UserName
        {
            get => GetSettingsValue("UserName", default(String));
            set => LocalSettings.Values["UserName"] = value;
        }

        public int UserInfoId
        {
            get => GetSettingsValue("UserInfoId", default(int));
            set => LocalSettings.Values["UserInfoId"] = value;
        }

        public UserInfoModel UserInfo
        {
            get
            {
                var jsonstring = GetSettingsValue("UserInfo", default(string));
                var model = JsonConvert.DeserializeObject<UserInfoModel>(jsonstring);
                return model;
            }
            set => LocalSettings.Values["UserInfo"] = JsonConvert.SerializeObject(value);
        }

        public string SQLServerConnectionString
        {
            get => GetSettingsValue("SQLServerConnectionString", GetConnectionString);
            set => SetSettingsValue("SQLServerConnectionString", value);
        }

        public bool IsRandomErrorsEnabled
        {
            get => GetSettingsValue("IsRandomErrorsEnabled", false);
            set => LocalSettings.Values["IsRandomErrorsEnabled"] = value;
        }

        public string WindowsHelloPublicKeyHint
        {
            get => GetSettingsValue("WindowsHelloPublicKeyHint", default(String));
            set => LocalSettings.Values["WindowsHelloPublicKeyHint"] = value;
        }

        private TResult GetSettingsValue<TResult>(string name, TResult defaultValue)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue;
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }
        private TResult GetSettingsValue<TResult>(string name, Func<TResult> action)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = action();
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return action();
            }
        }
        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }

        private string GetConnectionString()
        {
            string Datasource = GetRegValue("DBServer", "SQL5053.site4now.net");
            string Catalog = GetRegValue("DBCatalog", "DB_A637E6_LmsDev");
            string Username = GetRegValue("DBUsername", "DB_A637E6_LmsDev_admin");
            string Password = GetRegValue("DBPassword", "Matrix@291");
            string Trusted_Connection = GetRegValue("Trusted_Connection", "False");
            string Integrated_Security = GetRegValue("Integrated_Security", "False");

            string connectionString = "Data Source=" + Datasource + ";Initial Catalog=" + Catalog + ";User ID=" +
                                      Username + ";Password=" + Password + ";Trusted_Connection=" + Trusted_Connection + ";Integrated Security=" + Integrated_Security + ";Pooling=False"; 
            return connectionString;
        }

        private string GetRegValue(string key, string defaultValue = "")
        {
            var logService = ServiceLocator.Current.GetService<ILogService>();
            
            string regVal = defaultValue;
            try
            {
                RegistryKey reg = Registry.LocalMachine.OpenSubKey(REGPATH, false);
                if (reg == null)
                {
                    return regVal;
                }

                regVal = reg.GetValue(key).ToString();
            }
            catch (Exception ex)
            {

                logService.WriteAsync(Data.LogType.Information, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message,ex.InnerException?.Message);
            }
            
                return regVal;
            
            
        }
    }
}
