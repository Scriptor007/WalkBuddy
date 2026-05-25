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
            var onboardingDone = Preferences.Get(OnboardingDoneKey, false);
            Page root = onboardingDone
                ? new AppShell()
                : new NavigationPage(new Pages.OnboardingPage());

            return new Window(root);
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
