using System.Windows.Input;

namespace WalkBuddy.Pages.Controls;

public partial class GpsAccessCard : ContentView
{
    public static readonly BindableProperty AccessCommandProperty = BindableProperty.Create(
        nameof(AccessCommand),
        typeof(ICommand),
        typeof(GpsAccessCard),
        null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GpsAccessCard)bindable;
            control.SfButton.Command = (ICommand)newValue;
        });

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