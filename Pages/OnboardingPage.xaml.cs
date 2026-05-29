namespace WalkBuddy.Pages;

public partial class OnboardingPage : ContentPage
{
	public OnboardingPage(OnboardingPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

	public OnboardingPage()
	{
		InitializeComponent();
		var services = Application.Current?.Handler?.MauiContext?.Services;
		if (services != null)
		{
			BindingContext = new OnboardingPageModel(
				services.GetRequiredService<UserRepository>(),
				services.GetRequiredService<StaticDataService>());
		}
	}

    protected override bool OnBackButtonPressed()
    {
		return true;
    }
}
