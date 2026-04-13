using Godot;

public partial class MainMenu : CanvasLayer
{
    [Export] public TextureButton _play;
    [Export] public TextureButton _quit;
    [Export] public TextureButton _settings;
    [Export] public TextureButton _credits;
    [Export] public CanvasLayer _settingsmenu;

    [Export] public Texture2D _playFI;
    [Export] public Texture2D _quitFI;
    [Export] public Texture2D _settingsFI;
    [Export] public Texture2D _creditsFI;

    [Export] public Texture2D _playEN;
    [Export] public Texture2D _quitEN;
    [Export] public Texture2D _settingsEN;
    [Export] public Texture2D _creditsEN;

	public override void _Ready()
	{
		GetTree().CallGroup("GameHUD", "hide");
		_settingsmenu.Visible = false;
		_settings.Pressed += OnSettingsPressed;
		_play.Pressed += OnPlayPressed;
		_quit.Pressed += OnQuitPressed;

		GlobalSettings.Instance.LocaleChanged += RefreshTextures;
		RefreshTextures();
	}


	public override void _ExitTree()
	{
    GlobalSettings.Instance.LocaleChanged -= RefreshTextures;
}

    private void RefreshTextures()
{
    bool isFinnish = GlobalSettings.Instance.GetLocale() == "fi";
    _play.TextureNormal = isFinnish ? _playFI : _playEN;
    _quit.TextureNormal = isFinnish ? _quitFI : _quitEN;
    _settings.TextureNormal = isFinnish ? _settingsFI : _settingsEN;
    _credits.TextureNormal = isFinnish ? _creditsFI : _creditsEN;
}

    private void OnSettingsPressed()
    {
        _settingsmenu.Visible = true;
    }

    private void OnPlayPressed()
    {
        GetTree().CallGroup("GameHUD", "show");
        GetTree().ChangeSceneToFile("res://intro.tscn");
    }

    private void OnQuitPressed() => GetTree().Quit();

    // private void OnCreditsPressed()
    // {
    //     GetTree().ChangeSceneToFile("res://Scenes/Credits.tscn");
    // }
}