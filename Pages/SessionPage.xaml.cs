namespace WalkBuddy.Pages
{
    public partial class SessionPage : ContentPage
    {
        public SessionPage(SessionPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public SessionPage()
        {
            InitializeComponent();
            var services = Application.Current?.Handler?.MauiContext?.Services;
            if (services != null)
            {
                BindingContext = services.GetRequiredService<SessionPageModel>();
            }
        }
    }
}
