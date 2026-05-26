using Microsoft.Maui.Storage;

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
    }
}
