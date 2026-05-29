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
#if DEBUG
            Preferences.Set("onboarding_done", true);
#endif
            var onboardingDone = Preferences.Get(OnboardingDoneKey, false);

            var shell = new AppShell();
            if (!onboardingDone)
            {
                _ = shell.GoToAsync("//onboarding");
            }

            return new Window(shell);
        }

        public static void MarkOnboardingComplete()
        {
            Preferences.Set(OnboardingDoneKey, true);
            var window = Current?.Windows.FirstOrDefault();
            if (window != null)
            {
                window.Page = new AppShell();
            }
        }
    }
}
