using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkBuddy.Services
{
    public partial class TimerService : ObservableObject
    {
        private IDispatcherTimer? _timer;

        private DateTime _startTime;
        
        [ObservableProperty]
        private int _minPassed = -1;

        public void InitTimer()
        {
            IDispatcher? dispatcher = Dispatcher.GetForCurrentThread();

            if (dispatcher != null)
            {
                MinPassed = 0;
                _startTime = DateTime.Now;
                _timer = dispatcher.CreateTimer();
                _timer.Interval = TimeSpan.FromMinutes(1);
                _timer.Tick += TimerTick;
                _timer.Start();
            }
        }

        public void Stop()
        {
            _timer?.Stop();
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            UpdateMinutes();
        }

        public void UpdateMinutes()
        {
            TimeSpan minPassed = DateTime.Now - _startTime;
            MinPassed = (int)minPassed.TotalMinutes;
        }

        public void ResetTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= TimerTick;
                _timer = null;
            }

            MinPassed = -1;
            _startTime = DateTime.Now;
        }
    }
}
