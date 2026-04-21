using Godot;

public partial class Settings : CanvasLayer
{
    private Button _mainmenu;
    private Button _suomi;
    private Button _english;
    private SFXManager _sfx;

    public override void _Ready()
    {
        _sfx = GetNode<SFXManager>("/root/SFXManager");

        _mainmenu = GetNode<Button>("ColorRect/BACKTOMENU");
        _suomi = GetNode<Button>("ColorRect/SUOMI");
        _english = GetNode<Button>("ColorRect/ENGLISH");

        _mainmenu.Pressed += OnMenuPressed;
        _suomi.Pressed += OnSuomiPressed;
        _english.Pressed += OnEnglishPressed;
    }

    private void OnMenuPressed()
    {
        _sfx.PlayClick();
        Visible = false;
    }

    private void OnSuomiPressed()
    {
        _sfx.PlayClick();
        GlobalSettings.Instance.SetLocale("fi");
    }

    private void OnEnglishPressed()
    {
        _sfx.PlayClick();
        GlobalSettings.Instance.SetLocale("en");
    }
}