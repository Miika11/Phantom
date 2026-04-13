using Godot;

public partial class Settings : CanvasLayer
{
    private Button _mainmenu;
    private Button _suomi;
    private Button _english;

    public override void _Ready()
    {
        _mainmenu = GetNode<Button>("ColorRect/BACKTOMENU");
        _suomi = GetNode<Button>("ColorRect/SUOMI");
        _english = GetNode<Button>("ColorRect/ENGLISH");

        _mainmenu.Pressed += OnMenuPressed;
        _suomi.Pressed += () => {
            GlobalSettings.Instance.SetLocale("fi");
        };
        _english.Pressed += () => {
            GlobalSettings.Instance.SetLocale("en");
        };
    }

    private void OnMenuPressed()
    {
        Visible = false;
    }
}