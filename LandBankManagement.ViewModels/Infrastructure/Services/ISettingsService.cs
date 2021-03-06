﻿using LandBankManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LandBankManagement.Services
{
    public enum DataProviderType
    {
        SQLite,
        SQLServer,
        WebAPI
    }

    public interface ISettingsService
    {
        string Version { get; }
        string DbVersion { get; }
        
        DataProviderType DataProvider { get; set; }
        string PatternConnectionString { get; }
        string SQLServerConnectionString { get; set; }
        bool IsRandomErrorsEnabled { get; set; }

        Task<Result> ResetLocalDataProviderAsync();

        Task<Result> ValidateConnectionAsync(string connectionString);
        Task<Result> CreateDabaseAsync(string connectionString);
        List<SettingsDictionary> FetchAllLocalAppSettings();
        void ClearAllLocalAppSettings();
    }
}
