using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WalkBuddy.Models;
using WalkBuddy.Pages;

namespace WalkBuddy.PageModels
{
    [QueryProperty(nameof(MistralAnswer), "answer")]
    public partial class NaturePromptPageModel : ObservableObject
    {
        private readonly MistralService _mistralService;

        private readonly SessionRepository _sessionRepository;

        public TimerService TimerService { get; }

        private bool _isBusy;

        [ObservableProperty]
        private string _mistralAnswer = string.Empty;

        [ObservableProperty]
        private Session? _session = new();

        public NaturePromptPageModel(MistralService mistralService, SessionRepository sessionRepository, TimerService timerService)
        {
            _mistralService = mistralService;
            _sessionRepository = sessionRepository;
            TimerService = timerService;
        }

        [RelayCommand]
        private async Task InitialSessionLoad()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                int id = Preferences.Get("active_session_id", 0);
                Session = await _sessionRepository.GetSessionAsync(id);
                if (TimerService.MinPassed == -1)
                {
                    TimerService.InitTimer();
                }
            }
            finally
            {
                _isBusy = false;
            }
        }

        [RelayCommand]
        private async Task StopTimer()
        {
            TimerService.Stop();
        }

        [RelayCommand]
        private async Task FinishCurrentActivity()
        {
            await Shell.Current.GoToAsync(nameof(CheckInPage));
        }

        [RelayCommand]
        private async Task<int> Another()
        {
            //await _mistralService.AnotherRequest();
            return 1;
        }

        [RelayCommand]
        private async Task EndSession()
        {
            Preferences.Remove("active_session_id");
            await Shell.Current.GoToAsync(nameof(ReflectionPage));
        }
    }
}
