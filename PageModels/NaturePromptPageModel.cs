using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WalkBuddy.Models;
using WalkBuddy.Pages;
using WalkBuddy.Utilities;

namespace WalkBuddy.PageModels
{
    [QueryProperty(nameof(MistralAnswer), "answer")]
    public partial class NaturePromptPageModel : ObservableObject
    {
        private readonly MistralService _mistralService;

        private readonly SessionRepository _sessionRepository;

        private readonly TimerService _timerService;

        private bool _isBusy;

        [ObservableProperty]
        private string _mistralAnswer = string.Empty;

        [ObservableProperty]
        private Session? _session = new();

        public Locale? enLanguage { get; set; }

        public NaturePromptPageModel(MistralService mistralService, SessionRepository sessionRepository, TimerService timerService)
        {
            _mistralService = mistralService;
            _sessionRepository = sessionRepository;
            _timerService = timerService;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                WeakReferenceMessenger.Default.Register<TimerResumeMessage>(this, (sender, args) =>
                {
                    _timerService.UpdateMinutes();
                });
                int id = Preferences.Get("active_session_id", 0);
                Session = await _sessionRepository.GetSessionAsync(id);
                if (_timerService.MinPassed == -1)
                {
                    _timerService.InitTimer();
                }
            }
            finally
            {
                _isBusy = false;
            }
        }

        [RelayCommand]
        private async Task Disappearing()
        {
            WeakReferenceMessenger.Default.Unregister<TimerResumeMessage>(this);
        }

        [RelayCommand]
        private async Task FinishCurrentActivity()
        {
            await Shell.Current.GoToAsync(nameof(CheckInPage));
        }

        [RelayCommand]
        private async Task GetNewPrompt()
        {
            MistralAnswer = await _mistralService.GetNewPrompt();
        }

        [RelayCommand]
        private async Task EndSession()
        {
            Preferences.Remove("active_session_id");
            _timerService.Stop();
            await Shell.Current.GoToAsync(nameof(ReflectionPage));
        }

        [RelayCommand]
        private async Task VoiceOutput()
        {
            if (enLanguage == null)
            {
                IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();
                Locale? locale = locales.FirstOrDefault(l => l.Language.StartsWith("en"));
                enLanguage = locale;
            }

            SpeechOptions options = new() { Locale = enLanguage };

            await TextToSpeech.SpeakAsync(MistralAnswer, options);
        }
    }
}
