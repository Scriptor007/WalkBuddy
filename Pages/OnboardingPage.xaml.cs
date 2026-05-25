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
	}

	private void FinishOnboarding_Clicked(object sender, EventArgs e)
	{
		App.MarkOnboardingComplete();
	}
}
