namespace WalkBuddy.Pages;

public partial class NaturePromptPage : ContentPage
{
	public NaturePromptPage(NaturePromptPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

	private async void EndSession_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ReflectionPage));
	}
}
