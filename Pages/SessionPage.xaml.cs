namespace WalkBuddy.Pages
{
    public partial class SessionPage : ContentPage
    {
        public SessionPage(SessionPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}