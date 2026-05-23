namespace WalkBuddy.Pages.Controls;

public partial class DividerWithLabel : ContentView
{
    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
        nameof(LabelText),
        typeof(string),
        typeof(DividerWithLabel),
        string.Empty);

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public DividerWithLabel()
    {
        InitializeComponent();
        this.BindingContext = this;
    }
}