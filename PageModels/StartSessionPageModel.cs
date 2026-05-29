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
    public partial class StartSessionPageModel : ObservableObject
    {
        private readonly SessionRepository _sessionRepository;

        private readonly UserRepository _userRepository;

        private readonly MistralService _mistralService;

        public string[] Emotions { get; } = [];

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private Session _session = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateSessionCommand))]
        private string _startEmotion = string.Empty;

        [ObservableProperty]
        private User _user = new();

        public StartSessionPageModel(SessionRepository sessionRepository, UserRepository userRepository, MistralService mistralService)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _mistralService = mistralService;
            Emotions = Enum.GetNames(typeof(Emotion));
        }

        [RelayCommand]
        private async Task InitialDataLoad()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                User = await _userRepository.GetUserAsync();
                Session = new();
                StartEmotion = string.Empty;
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
                Session.StartEmotion = Enum.Parse<Emotion>(StartEmotion);
                Session currSession = await _sessionRepository.SaveSessionAsync(Session);
                string answer = await _mistralService.SendRequest(User, Session);
                var param = new Dictionary<string, object>
                {
                    { "answer", answer }
                };
                await Shell.Current.GoToAsync(nameof(NaturePromptPage), param);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanCreateSession() => !string.IsNullOrWhiteSpace(StartEmotion);
    }
}
