using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WalkBuddy.Data;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public partial class SessionPageModel : ObservableObject
    {
        private readonly SessionRepository _sessionRepository;
        private bool _isBusy;
        [ObservableProperty]
        private ObservableCollection<SessionCardModel> _sessions = new();
        [ObservableProperty]
        private string _headerText = "No walks recorded";
        public SessionPageModel(SessionRepository sessionRepository)
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
                var items = await _sessionRepository.ListSessionsAsync();
                Sessions.Clear();
                foreach (var session in items)
                {
                    Sessions.Add(SessionCardModel.FromSession(session));
                }
                HeaderText = Sessions.Count == 0
                    ? "No walks recorded"
                    : $"{Sessions.Count} walks recorded";
            }
            finally
            {
                _isBusy = false;
            }
        }
        [RelayCommand]
        private void NavigatedTo() { }
        [RelayCommand]
        private void NavigatedFrom() { }
    }
}