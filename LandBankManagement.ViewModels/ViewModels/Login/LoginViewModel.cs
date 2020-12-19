using System;
using System.Windows.Input;
using System.Threading.Tasks;

using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(ILoginService loginService, ISettingsService settingsService, ICommonServices commonServices) : base(commonServices)
        {
            LoginService = loginService;
            SettingsService = settingsService;
        }

        public ILoginService LoginService { get; }
        public ISettingsService SettingsService { get; }

        private ShellArgs ViewModelArgs { get; set; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private bool _isLoginWithPassword = false;
        public bool IsLoginWithPassword
        {
            get { return _isLoginWithPassword; }
            set { Set(ref _isLoginWithPassword, value); }
        }

        private bool _isLoginWithWindowsHello = false;
        public bool IsLoginWithWindowsHello
        {
            get { return _isLoginWithWindowsHello; }
            set { Set(ref _isLoginWithWindowsHello, value); }
        }

        private string _userName = null;
        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public ICommand ShowLoginWithPasswordCommand => new RelayCommand(ShowLoginWithPassword);
        public ICommand LoginWithPasswordCommand => new RelayCommand(LoginWithPassword);
        public ICommand LoginWithWindowHelloCommand => new RelayCommand(LoginWithWindowHello);
        private bool loginProcessStarted = false;
        public Task LoadAsync(ShellArgs args)
        {
            ViewModelArgs = args;

            UserName = args.UserInfo.UserName;
            IsLoginWithWindowsHello = LoginService.IsWindowsHelloEnabled(UserName);
            IsLoginWithPassword = !IsLoginWithWindowsHello;
            IsBusy = false;

            return Task.CompletedTask;
        }

        public void Login()
        {
            ShowProgressRing();
            if (IsLoginWithPassword)
            {
                LoginWithPassword();
            }
            else
            {
                LoginWithWindowHello();
            }
        }

        private void ShowLoginWithPassword()
        {
            IsLoginWithWindowsHello = false;
            IsLoginWithPassword = true;
        }

        public async void LoginWithPassword()
        {
            if (loginProcessStarted)
                return;

             loginProcessStarted = true;
            IsBusy = true;
           
            var result = ValidateInput();
            if (result.IsOk)
            {
                result = await LoginService.SignInWithPasswordAsync(UserName, Password);
                if (result.IsOk)
                {
                    EnterApplication();
                    loginProcessStarted = false;
                    return;
                }

            }
            loginProcessStarted = false;
            await DialogService.ShowAsync(result.Message, result.Description);
            IsBusy = false;
        }

        public async void LoginWithWindowHello()
        {
            IsBusy = true;
            var result = await LoginService.SignInWithWindowsHelloAsync();
            if (result.IsOk)
            {
                EnterApplication();
                return;
            }
            await DialogService.ShowAsync(result.Message, result.Description);
            IsBusy = false;
        }

        private void EnterApplication()
        {
            ViewModelArgs.UserInfo = LoginService.UserInfo;
            NavigationService.Navigate<MainShellViewModel>(ViewModelArgs);
        }

        private Result ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                return Result.Error("Login error", "Please, enter valid credentials.");
            }

            return Result.Ok();
        }
    }
}
