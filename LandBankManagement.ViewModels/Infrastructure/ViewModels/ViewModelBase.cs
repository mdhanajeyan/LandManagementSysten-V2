using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        private int _selectedPivotIndex;
        public int SelectedPivotIndex
        {
            get => _selectedPivotIndex;
            set => Set(ref _selectedPivotIndex, value);
        }

        private bool _progressRingVisibility;
        public bool ProgressRingVisibility
        {
            get => _progressRingVisibility;
            set => Set(ref _progressRingVisibility, value);
        }

        private bool _progressRingActive;
        public bool ProgressRingActive
        {
            get => _progressRingActive;
            set => Set(ref _progressRingActive, value);
        }

        private Stopwatch _stopwatch = new Stopwatch();

        public ViewModelBase(ICommonServices commonServices)
        {
            ContextService = commonServices.ContextService;
            NavigationService = commonServices.NavigationService;
            MessageService = commonServices.MessageService;
            DialogService = commonServices.DialogService;
            LogService = commonServices.LogService;
        }

        public IContextService ContextService { get; }
        public INavigationService NavigationService { get; }
        public IMessageService MessageService { get; }
        public IDialogService DialogService { get; }
        public ILogService LogService { get; }

        public bool IsMainView => ContextService.IsMainView;

        virtual public string Title => String.Empty;

        public async void ShowProgressRing() {
            ProgressRingActive = true;
            ProgressRingVisibility = true;
        }
        public void HideProgressRing()
        {
            ProgressRingActive = false;
            ProgressRingVisibility = false;
        }

        public async void LogInformation(string source, string action, string message, string description)
        {
            await LogService.WriteAsync(LogType.Information, source, action, message, description);
        }

        public async void LogWarning(string source, string action, string message, string description)
        {
            await LogService.WriteAsync(LogType.Warning, source, action, message, description);
        }

        public void LogException(string source, string action, Exception exception)
        {
            LogError(source, action, exception.Message, exception.ToString());
        }
        public async void LogError(string source, string action, string message, string description)
        {
            await LogService.WriteAsync(LogType.Error, source, action, message, description);
        }

        public void StartStatusMessage(string message)
        {
            StatusMessage(message);
            _stopwatch.Reset();
            _stopwatch.Start();
        }
        public void EndStatusMessage(string message)
        {
            _stopwatch.Stop();
            StatusMessage($"{message} ({_stopwatch.Elapsed.TotalSeconds:#0.000} seconds)");
        }

        public void StatusReady()
        {
            MessageService.Send(this, "StatusMessage", "Ready");
        }
        public void StatusMessage(string message)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(message);
            MessageService.Send(this, "StatusMessage", message);
        }
        public void StatusError(string message)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(message);
            MessageService.Send(this, "StatusError", message);
        }

        public void EnableThisView(string message = null)
        {
            message = message ?? "Ready";
            MessageService.Send(this, "EnableThisView", message);
        }
        public void DisableThisView(string message)
        {
            MessageService.Send(this, "DisableThisView", message);
        }

        public void EnableOtherViews(string message = null)
        {
            message = message ?? "Ready";
            MessageService.Send(this, "EnableOtherViews", message);
        }
        public void DisableOtherViews(string message)
        {
            MessageService.Send(this, "DisableOtherViews", message);
        }

        public void EnableAllViews(string message = null)
        {
            message = message ?? "Ready";
            MessageService.Send(this, "EnableAllViews", message);
        }
        public void DisableAllViews(string message)
        {
            MessageService.Send(this, "DisableAllViews", message);
        }
    }
}
