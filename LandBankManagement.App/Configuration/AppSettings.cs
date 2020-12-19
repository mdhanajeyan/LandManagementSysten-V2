using LandBankManagement.Models;
using LandBankManagement.Services;
using Newtonsoft.Json;
using System;
using System.IO;

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

        static public AppSettings Current { get; }

        static public readonly string AppLogPath = "AppLog";
        static public readonly string AppLogName = $"AppLog.1.0.db";
        static public readonly string AppLogFileName = Path.Combine(AppLogPath, AppLogName);

        static public readonly string DatabasePatternFileName = "";
        static public readonly string DatabasePath = "";
        static public readonly string DatabasePattern = "";
        static public readonly string DatabaseName = "";

        public readonly string AppLogConnectionString = $"Data Source={AppLogFileName}";

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
            get {
                var jsonstring = GetSettingsValue("UserInfo", default(string));
                var model = JsonConvert.DeserializeObject<UserInfoModel>(jsonstring);
                return model;
            }
            set => LocalSettings.Values["UserInfo"] = JsonConvert.SerializeObject(value);
        }

        public string SQLServerConnectionString
        {
            get => GetSettingsValue("SQLServerConnectionString", @"Data Source=SQL5053.site4now.net;Initial Catalog=DB_A637E6_LmsDev;User Id=DB_A637E6_LmsDev_admin;Password=Matrix@291;Pooling=False");
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
        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }
    }
}
