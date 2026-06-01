using CommunityToolkit.Maui;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using Syncfusion.Maui.Core.Hosting;
using System.Net.Http.Headers;
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
                .ConfigureSyncfusionCore()
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
            builder.Services.AddSingleton<StaticDataService>();
            builder.Services.AddSingleton<SessionRepository>();
            builder.Services.AddSingleton<StaticDataService>();
            builder.Services.AddSingleton<MistralService>();
            builder.Services.AddSingleton<TimerService>();

            builder.Services.AddTransient<SessionPageModel>();
            builder.Services.AddTransient<OnboardingPageModel>();
            builder.Services.AddTransient<EditProfilePageModel>();
            builder.Services.AddTransient<StartSessionPageModel>();
            builder.Services.AddTransient<NaturePromptPageModel>();
            builder.Services.AddTransient<CheckInPageModel>();
            builder.Services.AddTransient<ReflectionPageModel>();

            builder.Services.AddTransient<OnboardingPage>();
            builder.Services.AddTransient<EditProfilePage>();
            builder.Services.AddTransient<StartSessionPage>();
            builder.Services.AddTransient<NaturePromptPage>();
            builder.Services.AddTransient<CheckInPage>();

            builder.Services.AddHttpClient<MistralService>((serviceProvider, httpClient) =>
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "3sWny4kVbnK4plO6fewjlVgH5XWBbFNp");
                httpClient.BaseAddress = new Uri("https://api.mistral.ai");
            });
            builder.Services.AddTransient<ReflectionPage>();

            return builder.Build();
        }
    }
}
