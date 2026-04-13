using Godot;

public partial class PauseMenu : CanvasLayer
{
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;
        GetNode<Button>("Control/CONTINUE").Pressed += OnContinuePressed;
        GetNode<Button>("Control/BACKTOMENU").Pressed += OnMainMenuPressed;
        GetNode<Button>("Control/SUOMI").Pressed += () => GlobalSettings.Instance.SetLocale("fi");
        GetNode<Button>("Control/ENGLISH").Pressed += () => GlobalSettings.Instance.SetLocale("en");
    }

    public void TogglePause()
    {
        Visible = !Visible;
        GetTree().Paused = Visible;
    }

    private void OnContinuePressed() => TogglePause();

    private void OnMainMenuPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
}