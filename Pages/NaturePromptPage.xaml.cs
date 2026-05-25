namespace WalkBuddy.Pages;

public partial class NaturePromptPage : ContentPage
{
	public NaturePromptPage()
	{
		InitializeComponent();
	}

	private async void EndSession_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ReflectionPage));
	}
}
