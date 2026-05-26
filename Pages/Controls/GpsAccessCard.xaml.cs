using System.Windows.Input;

namespace WalkBuddy.Pages.Controls;

public partial class GpsAccessCard : ContentView
{
    public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(
        nameof(ButtonText),
        typeof(string),
        typeof(GpsAccessCard),
        string.Empty
        );

    public static readonly BindableProperty AccessCommandProperty = BindableProperty.Create(
        nameof(AccessCommand),
        typeof(ICommand),
        typeof(GpsAccessCard),
        null);

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public ICommand AccessCommand
    {
        get => (ICommand)GetValue(AccessCommandProperty);
        set => SetValue(AccessCommandProperty, value);
    }

    public GpsAccessCard()
    {
        InitializeComponent();
    }
}