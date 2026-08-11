using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using WalkBuddy.Data;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public partial class ReflectionPageModel : ObservableObject
    {
        private readonly SessionRepository _sessionRepository;
        private bool _isBusy;

        public IReadOnlyList<string> Emotions { get; } = Enum.GetNames(typeof(Emotion));

        public TimerService TimerService { get; }

        [ObservableProperty]
        private Session? _session;

        [ObservableProperty]
        private string _selectedEmotionName = string.Empty;

        [ObservableProperty]
        private string _note = string.Empty;

        [ObservableProperty]
        private string _subtitleText = "Add a short reflection (optional)";

        public ReflectionPageModel(SessionRepository sessionRepository, TimerService timerService)
        {
            _sessionRepository = sessionRepository;
            TimerService = timerService;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                var id = Preferences.Get("active_session_id", 0);
                if (id != 0)
                {
                    Session = await _sessionRepository.GetSessionAsync(id);
                }
                else
                {
                    Session = await _sessionRepository.GetLatestSessionAsync();
                }

                if (Session != null)
                {
                    SelectedEmotionName = Session.EndEmotion.ToString();
                    Note = Session.Note;
                }
            }
            finally
            {
                _isBusy = false;
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                if (Session == null)
                {
                    Session = await _sessionRepository.GetLatestSessionAsync()
                        ?? new Session { CreatedAt = DateTime.Now };
                }

                if (Enum.TryParse<Emotion>(SelectedEmotionName, out var parsedEmotion))
                {
                    Session.EndEmotion = parsedEmotion;
                }
                Session.Duration = TimerService.MinPassed == -1 ? 0 : TimerService.MinPassed;
                Session.Note = Note ?? string.Empty;
                await _sessionRepository.SaveSessionAsync(Session);
                Preferences.Set("active_session_id", 0);
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync("//sessions");
            }
            finally
            {
                _isBusy = false;
            }
        }

        [RelayCommand]
        private async Task Skip()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                if (Session == null)
                {
                    Session = await _sessionRepository.GetLatestSessionAsync()
                        ?? new Session { CreatedAt = DateTime.Now };
                }

                Session.Duration = TimerService.MinPassed == -1 ? 0 : TimerService.MinPassed;
                await _sessionRepository.SaveSessionAsync(Session);
                Preferences.Set("active_session_id", 0);
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync("//sessions");
            }
            finally
            {
                _isBusy = false;
            }
        }
    }
}
