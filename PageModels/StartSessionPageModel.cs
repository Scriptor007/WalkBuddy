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

        public string[] Emotions { get; } = [];

        private bool _isBusy;

        [ObservableProperty]
        private Session _session = new();

        [ObservableProperty]
        private User _user = new();

        public StartSessionPageModel(SessionRepository sessionRepository, UserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            Emotions = Enum.GetNames(typeof(Emotion));
        }

        [RelayCommand]
        private async Task InitialDataLoad()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                User = await _userRepository.GetUserAsync();
            }
            finally
            {
                _isBusy = false;
            }
        }

        [RelayCommand]
        private async Task CreateSession()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                await _sessionRepository.SaveSessionAsync(Session);
                await Shell.Current.GoToAsync(nameof(NaturePromptPage));
            }
            finally
            {
                _isBusy = false;
            }
        }
    }
}
