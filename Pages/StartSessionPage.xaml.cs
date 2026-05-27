namespace WalkBuddy.Pages;

public partial class StartSessionPage : ContentPage
{
	public StartSessionPage(StartSessionPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

	private async void StartWalk_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(NaturePromptPage));
	}
}
