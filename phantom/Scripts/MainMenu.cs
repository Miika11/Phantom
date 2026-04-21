using Godot;

public partial class MainMenu : CanvasLayer
{
    [Export] public TextureButton _play;
    [Export] public TextureButton _quit;
    [Export] public TextureButton _settings;
    [Export] public TextureButton _credits;
    [Export] public CanvasLayer _settingsmenu;
    [Export] public CanvasLayer _creditsmenu;

    [Export] public TextureButton _creditsClose;

    [Export] public Texture2D _creditsImageFI;
    [Export] public Texture2D _creditsImageEN;
    [Export] public TextureRect _creditsImage;

    [Export] public Texture2D _playFI;
    [Export] public Texture2D _quitFI;
    [Export] public Texture2D _settingsFI;
    [Export] public Texture2D _creditsFI;

    [Export] public Texture2D _playEN;
    [Export] public Texture2D _quitEN;
    [Export] public Texture2D _settingsEN;
    [Export] public Texture2D _creditsEN;

    private SFXManager _sfx;

    public override void _Ready()
    {
        GetNode<MusicManager>("/root/MusicManager")
            .PlayMusic("res://audio/MainMenu.ogg");

        _sfx = GetNode<SFXManager>("/root/SFXManager");

        GetTree().CallGroup("GameHUD", "hide");

        _settingsmenu.Visible = false;
        _creditsmenu.Visible = false;

        _settings.Pressed += OnSettingsPressed;
        _play.Pressed += OnPlayPressed;
        _quit.Pressed += OnQuitPressed;
        _credits.Pressed += OnCreditsPressed;
        _creditsClose.Pressed += OnCreditsClosePressed;

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

        if (_creditsImage != null)
            _creditsImage.Texture = isFinnish ? _creditsImageFI : _creditsImageEN;
    }

    private void OnSettingsPressed()
    {
        _sfx.PlayClick();
        _settingsmenu.Visible = true;
    }

    private void OnCreditsPressed()
    {
        _sfx.PlayClick();
        _creditsmenu.Visible = true;
    }

    private void OnCreditsClosePressed()
    {
        _sfx.PlayClick();
        _creditsmenu.Visible = false;
    }

    private void OnPlayPressed()
    {
        _sfx.PlayClick();
        GetTree().CallGroup("GameHUD", "show");
        GetTree().ChangeSceneToFile("res://intro.tscn");
    }

    private void OnQuitPressed()
    {
        _sfx.PlayClick();
        GetTree().Quit();
    }
}