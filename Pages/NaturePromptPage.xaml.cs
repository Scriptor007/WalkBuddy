using System.Diagnostics;

namespace WalkBuddy.Pages;

public partial class NaturePromptPage : ContentPage
{
	private readonly TimerService _timerService;

	public NaturePromptPage(NaturePromptPageModel model, TimerService timerService)
	{
		InitializeComponent();
		NavigatedTo += OnNavigatedTo;
		BindingContext = model;
        _timerService = timerService;

    }

	private async void EndSession_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ReflectionPage));
	}

    void OnNavigatedTo(object sender, NavigatedToEventArgs args)
    {
        Page? previousPage = args.PreviousPage;
		if (previousPage is StartSessionPage)
		{
			_timerService.ResetTimer();
            _timerService.InitTimer();
        }
    }
}
