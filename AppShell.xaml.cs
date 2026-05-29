using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using WalkBuddy.Pages;
using Font = Microsoft.Maui.Font;

namespace WalkBuddy
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            var currentTheme = Application.Current!.RequestedTheme;
            //Preferences.Clear();
            Routing.RegisterRoute(nameof(Pages.NaturePromptPage), typeof(Pages.NaturePromptPage));
            Routing.RegisterRoute(nameof(Pages.ReflectionPage), typeof(Pages.ReflectionPage));

            if (!Preferences.Default.Get("isRegistered", false))
            {
                CurrentItem = this.Items[0];
            }
            else
            {
                CurrentItem = this.Items[1];
            }
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(0),
                Font = Font.SystemFontOfSize(18),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            // Toast is currently not working in MCT on Windows
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }

        private void SfSegmentedControl_SelectionChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }
    }
}
