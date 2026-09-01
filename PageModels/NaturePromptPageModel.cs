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

        private readonly PromptRepository _promptRepository;

        public TimerService TimerService { get; }

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _mistralAnswer = string.Empty;

        [ObservableProperty]
        private Session? _session = new();

        public Locale? enLanguage { get; set; }

        private readonly Stopwatch _screenOnStopwatch = new();

        private readonly Stopwatch _interactionStopwatch = new();
        private bool _hasInteracted = false;

        [ObservableProperty]
        private PromptLog _currentPromptLog = new();

        public NaturePromptPageModel(MistralService mistralService, SessionRepository sessionRepository, TimerService timerService, PromptRepository promptRepository)
        {
            _mistralService = mistralService;
            _sessionRepository = sessionRepository;
            TimerService = timerService;
            _promptRepository = promptRepository;
        }

        partial void OnMistralAnswerChanging(string? oldValue, string? newValue)
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                CurrentPromptLog = new PromptLog
                {
                    PromptText = newValue
                };

                _hasInteracted = false;
                _interactionStopwatch.Restart();
                _screenOnStopwatch.Restart();
            }
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                _screenOnStopwatch.Start();

                var window = Application.Current?.Windows.FirstOrDefault();
                if (window != null)
                {
                    window.Deactivated += OnAppDeactivated;
                    window.Activated += OnAppActivated;
                }

                WeakReferenceMessenger.Default.Register<TimerResumeMessage>(this, (sender, args) =>
                {
                    TimerService.UpdateMinutes();
                });
                int id = Preferences.Get("active_session_id", 0);
                Session = await _sessionRepository.GetSessionAsync(id);
                if (TimerService.MinPassed == -1)
                {
                    TimerService.InitTimer();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Disappearing()
        {
            WeakReferenceMessenger.Default.Unregister<TimerResumeMessage>(this);

            _screenOnStopwatch.Stop();

            var window = Application.Current?.Windows.FirstOrDefault();
            if (window != null)
            {
                window.Deactivated -= OnAppDeactivated;
                window.Activated -= OnAppActivated;
            }

            CurrentPromptLog.ScreenOnTimeSeconds = _screenOnStopwatch.Elapsed.TotalSeconds;
            if (!string.IsNullOrWhiteSpace(CurrentPromptLog.PromptText))
            {
                await _promptRepository.SaveSessionAsync(CurrentPromptLog);
            }
        }

        [RelayCommand]
        private async Task FinishCurrentActivity()
        {
            if (IsBusy)
                return;

            RegisterInteraction();
            CurrentPromptLog.UsedMidWalkCheckIn = true;
            var param = new Dictionary<string, object>
                {
                    { "answer", MistralAnswer }
                };
            await Shell.Current.GoToAsync(nameof(CheckInPage), param);
        }

        [RelayCommand]
        private async Task GetNewPrompt()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            RegisterInteraction();
            CurrentPromptLog.WasRejected = true;
            CurrentPromptLog.ScreenOnTimeSeconds = _screenOnStopwatch.Elapsed.TotalSeconds;
            if (!string.IsNullOrWhiteSpace(CurrentPromptLog.PromptText))
            {
                await _promptRepository.SaveSessionAsync(CurrentPromptLog);
            }

            try
            {
                MistralAnswer = await _mistralService.GetNewPrompt();
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
            if (IsBusy)
                return;

            RegisterInteraction();
            Preferences.Remove("active_session_id");
            TimerService.Stop();
            await Shell.Current.GoToAsync(nameof(ReflectionPage));
        }

        [RelayCommand]
        private async Task VoiceOutput()
        {
            RegisterInteraction();
            CurrentPromptLog.UsedAudioOutput = true;

            if (enLanguage == null)
            {
                IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();
                Locale? locale = locales.FirstOrDefault(l => l.Language.StartsWith("en"));
                enLanguage = locale;
            }

            SpeechOptions options = new() { Locale = enLanguage };

            await TextToSpeech.SpeakAsync(MistralAnswer, options);
        }

        private void RegisterInteraction()
        {
            if (!_hasInteracted && _interactionStopwatch.IsRunning)
            {
                _interactionStopwatch.Stop();
                _hasInteracted = true;

                CurrentPromptLog.TimeToFirstInteractionSeconds = _interactionStopwatch.Elapsed.TotalSeconds;
            }
        }

        private void OnAppDeactivated(object? sender, EventArgs e)
        {
            _screenOnStopwatch.Stop();
        }

        private void OnAppActivated(object? sender, EventArgs e)
        {
            _screenOnStopwatch.Start();
        }
    }
}
