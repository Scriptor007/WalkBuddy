using WalkBuddy.Models;
using WalkBuddy.PageModels;

namespace WalkBuddy.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}