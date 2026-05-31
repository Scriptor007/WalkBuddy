using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WalkBuddy.Models;
using WalkBuddy.Pages;
using WalkBuddy.Utilities;

namespace WalkBuddy.PageModels
{
    public partial class StartSessionPageModel : ObservableObject
    {
        private readonly SessionRepository _sessionRepository;

        private readonly UserRepository _userRepository;

        private readonly StaticDataService _staticDataService;

        private readonly MistralService _mistralService;

        public List<EmotionWithIcon> Emotions { get; } = [];

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private Session _session = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateSessionCommand))]
        private EmotionWithIcon _startEmotion = new();

        [ObservableProperty]
        private string _walkStats = "🌿 No walk completed yet";

        [ObservableProperty]
        private User _user = new();

        public StartSessionPageModel(SessionRepository sessionRepository, UserRepository userRepository, MistralService mistralService, StaticDataService staticDataService)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _mistralService = mistralService;
            _staticDataService = staticDataService;
            Emotions = _staticDataService.Emotions;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                User = await _userRepository.GetUserAsync();
                Session = new();
                StartEmotion = new EmotionWithIcon();
                int totalWalks = await _sessionRepository.GetTotalSessions();

                if (totalWalks > 0)
                {
                    Session? latestSession = await _sessionRepository.GetLatestSessionAsync();

                    if (latestSession != null)
                    {
                        string relativeTime = GetLatestWalkDay(latestSession.CreatedAt);

                        WalkStats = $"🌿 {totalWalks} walks completed  •  Last walk {relativeTime}";
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanCreateSession))]
        private async Task CreateSession()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Session.StartEmotion = Enum.Parse<Emotion>(StartEmotion.Emotion!);
                await _sessionRepository.SaveSessionAsync(Session);
                string answer = await _mistralService.SendRequest(User, Session);
                var param = new Dictionary<string, object>
                {
                    { "answer", answer }
                };
                Preferences.Set("active_session_id", Session.ID);
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

        private string GetLatestWalkDay(DateTime dateTime)
        {
            TimeSpan difference = DateTime.Now - dateTime;
            if (difference.TotalDays >= 2)
                return $"{Math.Floor(difference.TotalDays)} days ago";

            if (difference.TotalDays >= 1)
                return "Yesterday";

            return "Today";
        }

        private bool CanCreateSession() => !string.IsNullOrWhiteSpace(StartEmotion.Emotion);
    }
}
