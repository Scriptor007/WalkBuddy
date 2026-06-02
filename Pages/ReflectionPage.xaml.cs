using WalkBuddy.PageModels;

namespace WalkBuddy.Pages;

public partial class ReflectionPage : ContentPage
{
	public ReflectionPage(ReflectionPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

	public ReflectionPage()
	{
		InitializeComponent();
		var services = Application.Current?.Handler?.MauiContext?.Services;
		if (services != null)
		{
			BindingContext = services.GetRequiredService<ReflectionPageModel>();
		}
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Shell.Current.GoToAsync("//start");
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}
