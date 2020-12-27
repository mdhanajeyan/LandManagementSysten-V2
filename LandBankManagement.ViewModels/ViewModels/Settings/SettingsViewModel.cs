#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    #region SettingsArgs
    public class SettingsArgs
    {
        static public SettingsArgs CreateDefault() => new SettingsArgs();
    }
    #endregion

    public class SettingsDictionary
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(ILoginService loginService, ISettingsService settingsService, ICommonServices commonServices) : base(commonServices)
        {
            SettingsService = settingsService;
            LoginService = loginService;
        }

        public ISettingsService SettingsService { get; }

        public ILoginService LoginService { get; }

        public string Version => $"v{SettingsService.Version}";

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }


        public ICommand ViewAllSettingsCommand => new RelayCommand(OnViewAllSettings);
        public ICommand ClearAllSettingsCommand => new RelayCommand(OnClearAllSettings);

        public SettingsArgs ViewModelArgs { get; private set; }

        public List<SettingsDictionary> AppSettingsStorage { get; set; }

        public Task LoadAsync(SettingsArgs args)
        {
            ViewModelArgs = args ?? SettingsArgs.CreateDefault();

            StatusReady();

            AppSettingsStorage = SettingsService.FetchAllLocalAppSettings();

            return Task.CompletedTask;
        }



        private void OnViewAllSettings()
        {
            StatusReady();

            AppSettingsStorage = SettingsService.FetchAllLocalAppSettings();

            StatusError("Error Viewing All Settings");

        }

        private async void OnClearAllSettings()
        {
            var result = await DialogService.ShowAsync("Do you really want to clear all Settings? ", " You will be logged out and will have to login back if you choose to clear all current Settings.", "Yes", "No");

            if (result)
            {
                SettingsService.ClearAllLocalAppSettings();

                LoginService.Logoff();
            }

        }
    }
}
