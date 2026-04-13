using Godot;

public partial class GlobalSettings : Node
{
    public static GlobalSettings Instance { get; private set; }
    private string _currentLocale = "en";

    [Signal] public delegate void LocaleChangedEventHandler();

    public override void _Ready()
    {
        Instance = this;
    }

    public void SetLocale(string locale)
    {
        _currentLocale = locale;
        TranslationServer.SetLocale(locale);
        EmitSignal(SignalName.LocaleChanged);
    }

    public string GetLocale() => _currentLocale;
}