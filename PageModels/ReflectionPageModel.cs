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

        public string[] Emotions { get; } = Enum.GetNames(typeof(Emotion));

        [ObservableProperty]
        private Session? _session;

        [ObservableProperty]
        private Emotion _selectedEmotion;

        [ObservableProperty]
        private string _note = string.Empty;

        [ObservableProperty]
        private string _subtitleText = "Add a short reflection (optional)";

        public ReflectionPageModel(SessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
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
                    SelectedEmotion = Session.EndEmotion;
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
            if (_isBusy || Session == null)
                return;

            _isBusy = true;

            try
            {
                Session.EndEmotion = SelectedEmotion;
                Session.Note = Note ?? string.Empty;
                await _sessionRepository.SaveSessionAsync(Session);
                Preferences.Set("active_session_id", 0);
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
            Preferences.Set("active_session_id", 0);
            await Shell.Current.GoToAsync("//sessions");
        }
    }
}
