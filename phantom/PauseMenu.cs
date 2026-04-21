using Godot;

public partial class PauseMenu : CanvasLayer
{
    private SFXManager _sfx;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;

        _sfx = GetNode<SFXManager>("/root/SFXManager");

        GetNode<Button>("Control/CONTINUE").Pressed += OnContinuePressed;
        GetNode<Button>("Control/BACKTOMENU").Pressed += OnMainMenuPressed;
        GetNode<Button>("Control/SUOMI").Pressed += OnSuomiPressed;
        GetNode<Button>("Control/ENGLISH").Pressed += OnEnglishPressed;
    }

    public void TogglePause()
    {
        Visible = !Visible;
        GetTree().Paused = Visible;
    }

    private void OnContinuePressed() => TogglePause();

    private void OnMainMenuPressed()
    {
        _sfx.PlayClick();
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
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