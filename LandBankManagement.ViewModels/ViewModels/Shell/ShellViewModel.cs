using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class ShellArgs
    {
        public Type ViewModel { get; set; }
        public object Parameter { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(ILoginService loginService, ICommonServices commonServices) : base(commonServices)
        {
            IsLocked = !loginService.IsAuthenticated;
        }

        private bool _isLocked = false;
        public bool IsLocked
        {
            get => _isLocked;
            set => Set(ref _isLocked, value);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        private string _message = "Ready";
        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        private bool _isError = false;
        public bool IsError
        {
            get => _isError;
            set => Set(ref _isError, value);
        }

        private bool _showSuccessPopupMessage = false;
        public bool ShowSuccessPopupMessage
        {
            get => _showSuccessPopupMessage;
            set => Set(ref _showSuccessPopupMessage, value);
        }
        private bool _showErrorPopupMessage = false;
        public bool ShowErrorPopupMessage
        {
            get => _showErrorPopupMessage;
            set => Set(ref _showErrorPopupMessage, value);
        }

        private string _popupMessage;
        public string PopupMessage
        {
            get => _popupMessage;
            set => Set(ref _popupMessage, value);
        }

        private string _statusColor;
        public string StatusColor
        {
            get => _statusColor;
            set => Set(ref _statusColor, value);
        }


        public ShellArgs ViewModelArgs { get; protected set; }

        virtual public Task LoadAsync(ShellArgs args)
        {
            ViewModelArgs = args;
            if (ViewModelArgs != null)
            {
                //UserInfo = ViewModelArgs.UserInfo;
                NavigationService.Navigate(ViewModelArgs.ViewModel, ViewModelArgs.Parameter);
            }
            return Task.CompletedTask;
        }
        virtual public void Unload()
        {
        }

        virtual public void Subscribe()
        {
            MessageService.Subscribe<ILoginService, bool>(this, OnLoginMessage);
            MessageService.Subscribe<ViewModelBase, string>(this, OnMessage);
        }

        virtual public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        private async void OnLoginMessage(ILoginService loginService, string message, bool isAuthenticated)
        {
            if (message == "AuthenticationChanged")
            {
                await ContextService.RunAsync(() =>
                {
                    IsLocked = !isAuthenticated;
                });
            }
        }

        private async void OnMessage(ViewModelBase viewModel, string message, string status)
        {
            switch (message)
            {
                case "StatusMessage":
                case "StatusError":
                    if (viewModel.ContextService.ContextID == ContextService.ContextID)
                    {
                        IsError = message == "StatusError";
                        SetStatus(status);
                    }
                    break;

                case "EnableThisView":
                case "DisableThisView":
                    if (viewModel.ContextService.ContextID == ContextService.ContextID)
                    {
                        IsEnabled = message == "EnableThisView";
                        SetStatus(status);
                    }
                    break;

                case "EnableOtherViews":
                case "DisableOtherViews":
                    if (viewModel.ContextService.ContextID != ContextService.ContextID)
                    {
                        await ContextService.RunAsync(() =>
                        {
                            IsEnabled = message == "EnableOtherViews";
                            SetStatus(status);
                        });
                    }
                    break;

                case "EnableAllViews":
                case "DisableAllViews":
                    await ContextService.RunAsync(() =>
                    {
                        IsEnabled = message == "EnableAllViews";
                        SetStatus(status);
                    });
                    break;
                case "PopupSuccessMessage":
                  
                    PopupMessage = status;
                    ShowSuccessPopupMessage = true;
                    await Task.Delay(TimeSpan.FromSeconds(7));
                    ShowSuccessPopupMessage = false;
                    break;
                case "PopupErrorMessage":
                    PopupMessage = status;
                    ShowErrorPopupMessage = true;
                    await Task.Delay(TimeSpan.FromSeconds(7));
                    ShowErrorPopupMessage = false;
                    break;
            }
        }

        private void SetStatus(string message)
        {
            message = message ?? "";
            message = message.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
            Message = message;
        }
    }
}
