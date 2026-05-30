using System;
using System.Collections.Generic;
using System.Text;

namespace WalkBuddy.PageModels
{
    public class CheckInPageModel
    {
        public TimerService TimerService { get; }

        public CheckInPageModel(TimerService timerService)
        {
            TimerService = timerService;
        }
    }
}
