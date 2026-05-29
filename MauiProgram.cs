using CommunityToolkit.Maui;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using WalkBuddy.Pages;

namespace WalkBuddy
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(options =>
                {
#if WINDOWS
                    options.SetShouldEnableSnackbarOnWindows(true);
#endif
                })
                .ConfigureSyncfusionToolkit()
                .UseMaterialMauiIcons()
                .ConfigureMauiHandlers(handlers =>
                {
#if WINDOWS
    				Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper.AppendToMapping("KeyboardAccessibleCollectionView", (handler, view) =>
    				{
    					handler.PlatformView.SingleSelectionFollowsFocus = false;
    				});
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialSymbols.ttf", "MaterialSymbols");
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            builder.Services.AddSingleton<CategoryRepository>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<UserRepository>();
            builder.Services.AddSingleton<SessionRepository>();
            builder.Services.AddSingleton<StaticDataService>();

            builder.Services.AddTransient<MainPageModel>();
            builder.Services.AddTransient<OnboardingPageModel>();
            builder.Services.AddTransient<EditProfilePageModel>();
            builder.Services.AddTransient<StartSessionPageModel>();

            builder.Services.AddTransient<OnboardingPage>();
            builder.Services.AddTransient<EditProfilePage>();
            builder.Services.AddTransient<StartSessionPage>();

            return builder.Build();
        }
    }
}
