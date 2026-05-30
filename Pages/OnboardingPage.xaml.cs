namespace WalkBuddy.Pages;

public partial class OnboardingPage : ContentPage
{
	public OnboardingPage(OnboardingPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

    protected override bool OnBackButtonPressed()
    {
		return true;
    }
}
