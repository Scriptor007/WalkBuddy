using System.Diagnostics;

namespace WalkBuddy.Pages;

public partial class EditProfilePage : ContentPage
{
	public EditProfilePage(EditProfilePageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}