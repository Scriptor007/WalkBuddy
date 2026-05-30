using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    [QueryProperty(nameof(MistralAnswer), "answer")]
    public partial class NaturePromptPageModel : ObservableObject
    {
        private readonly MistralService _mistralService;

        private readonly SessionRepository _sessionRepository;

        private IDispatcherTimer? _timer;

        private DateTime _startTime = DateTime.Now; 

        private bool _isBusy;

        [ObservableProperty]
        private string _mistralAnswer = string.Empty;

        [ObservableProperty]
        private Session? _session = new();
        [ObservableProperty]        
        private int _minPassed; 

        public NaturePromptPageModel(MistralService mistralService, SessionRepository sessionRepository)
        {
            _mistralService = mistralService;
            _sessionRepository = sessionRepository;
            InitTimer();
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
                _timer?.Start();
                UpdateMinutes();
            }
            finally
            {
                _isBusy = false;
            }
        }

        [RelayCommand]
        private async Task StopTimer()
        {
            _timer?.Stop();
        }

        [RelayCommand]
        private async Task<int> Another()
        {
            //await _mistralService.AnotherRequest();
            return 1;
        }

        private void InitTimer()
        {
            IDispatcher? dispatcher = Dispatcher.GetForCurrentThread();

            if (dispatcher != null)
            {
                _timer = dispatcher.CreateTimer();
                _timer.Interval = TimeSpan.FromMinutes(1);
                _timer.Tick += TimerTick;
            }
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            UpdateMinutes();
        }

        private void UpdateMinutes()
        {
            TimeSpan minPassed = DateTime.Now - _startTime;
            MinPassed = (int)minPassed.TotalMinutes;
        }
    }
}
