using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;
using WalkBuddy.Utilities;

namespace WalkBuddy
{
    public partial class App : Application
    {
        private const string OnboardingDoneKey = "onboarding_done";

        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnResume()
        {
            base.OnResume();
            WeakReferenceMessenger.Default.Send(new TimerResumeMessage());
        }
    }
}
