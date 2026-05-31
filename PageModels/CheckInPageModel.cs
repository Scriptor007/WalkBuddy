using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using WalkBuddy.Utilities;

namespace WalkBuddy.PageModels
{
    public partial class CheckInPageModel : ObservableObject
    {
        public TimerService TimerService { get; }

        public CheckInPageModel(TimerService timerService)
        {
            TimerService = timerService;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            WeakReferenceMessenger.Default.Register<TimerResumeMessage>(this, (sender, args) =>
            {
                TimerService.UpdateMinutes();
            });
        }

        [RelayCommand]
        private async Task Disappearing()
        {
            WeakReferenceMessenger.Default.Unregister<TimerResumeMessage>(this);
        }
    }
}
