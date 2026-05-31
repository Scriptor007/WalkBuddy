namespace WalkBuddy.Pages;

public partial class CheckInPage : ContentPage
{
	public CheckInPage(CheckInPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}