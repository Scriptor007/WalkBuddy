namespace WalkBuddy.Pages;

public partial class StartSession : ContentPage
{
	public StartSession()
	{
		InitializeComponent();
	}

	private async void StartWalk_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(NaturePromptPage));
	}
}
