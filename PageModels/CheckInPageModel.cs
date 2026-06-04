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
        public TimerService TimerService { get; }

        private readonly MistralService _mistralService;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextPromptCommand))]
        private SfChip? _checkIn;

        public CheckInPageModel(TimerService timerService, MistralService mistralService)
        {
            TimerService = timerService;
            _mistralService = mistralService;
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

        [RelayCommand(CanExecute = nameof(CanGetNextPrompt))]
        private async Task NextPrompt()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                string mistralAnswer = await _mistralService.GetNewPrompt(CheckIn?.Text);
                var param = new Dictionary<string, object>
                {
                    { "answer", mistralAnswer }
                };
                await Shell.Current.GoToAsync(nameof(NaturePromptPage), param);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task EndSession()
        {
            Preferences.Remove("active_session_id");
            TimerService.Stop();
            await Shell.Current.GoToAsync(nameof(ReflectionPage));
        }

        private bool CanGetNextPrompt() => !string.IsNullOrWhiteSpace(CheckIn?.Text);
    }
}
