namespace WalkBuddy.Pages;

public partial class StartSessionPage : ContentPage
{
	public StartSessionPage(StartSessionPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}
