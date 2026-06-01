using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.Maui.Toolkit.Chips;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WalkBuddy.Pages;
using WalkBuddy.Utilities;

namespace WalkBuddy.PageModels
{
    public partial class CheckInPageModel : ObservableObject
    {
        private readonly TimerService _timerService;

        private readonly StaticDataService _staticDataService;

        public List<string> CheckInFeelings { get; } = [];

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextPromptCommand))]
        private SfChip? _checkIn;

        public CheckInPageModel(TimerService timerService, StaticDataService staticDataService)
        {
            _timerService = timerService;
            _staticDataService = staticDataService;
            CheckInFeelings = _staticDataService.CheckInFeelings;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            WeakReferenceMessenger.Default.Register<TimerResumeMessage>(this, (sender, args) =>
            {
                _timerService.UpdateMinutes();
            });
        }

        [RelayCommand]
        private async Task Disappearing()
        {
            WeakReferenceMessenger.Default.Unregister<TimerResumeMessage>(this);
        }

        [RelayCommand(CanExecute = nameof(CanGetNextPrompt))]
        private async Task NextPrompt()
        {

        }

        [RelayCommand]
        private async Task EndSession()
        {
            Preferences.Remove("active_session_id");
            _timerService.Stop();
            await Shell.Current.GoToAsync(nameof(ReflectionPage));
        }

        private bool CanGetNextPrompt() => !string.IsNullOrWhiteSpace(CheckIn?.Text);
    }
}
