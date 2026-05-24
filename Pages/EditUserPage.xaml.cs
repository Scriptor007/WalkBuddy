namespace WalkBuddy.Pages;

public partial class EditUserPage : ContentPage
{
    public EditUserPage(MainPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}