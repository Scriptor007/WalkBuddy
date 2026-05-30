using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkBuddy.PageModels
{
    public partial class CheckInPageModel : ObservableObject
    {
        public TimerService TimerService { get; }

        public CheckInPageModel(TimerService timerService)
        {
            TimerService = timerService;
        }
    }
}
