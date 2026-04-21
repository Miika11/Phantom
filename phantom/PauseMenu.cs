using Godot;

public partial class PauseMenu : CanvasLayer
{
    private SFXManager _sfx;

    private HSlider _musicSlider;
    private HSlider _sfxSlider;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;

        _sfx = GetNode<SFXManager>("/root/SFXManager");

        GetNode<Button>("Control/CONTINUE").Pressed += OnContinuePressed;
        GetNode<Button>("Control/BACKTOMENU").Pressed += OnMainMenuPressed;
        GetNode<Button>("Control/SUOMI").Pressed += OnSuomiPressed;
        GetNode<Button>("Control/ENGLISH").Pressed += OnEnglishPressed;

        _musicSlider = GetNode<HSlider>("Control/MusicSlider");
        _sfxSlider = GetNode<HSlider>("Control/SFXSlider");


        _musicSlider.Value = GlobalSettings.Instance.GetMusicVolume();
        _sfxSlider.Value = GlobalSettings.Instance.GetSFXVolume();

        _musicSlider.ValueChanged += OnMusicVolumeChanged;
        _sfxSlider.ValueChanged += OnSFXVolumeChanged;
    }

    public void TogglePause()
    {
        Visible = !Visible;
        GetTree().Paused = Visible;


        if (Visible)
        {
            _musicSlider.Value = GlobalSettings.Instance.GetMusicVolume();
            _sfxSlider.Value = GlobalSettings.Instance.GetSFXVolume();
        }
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

    private void OnMusicVolumeChanged(double value)
    {
        GlobalSettings.Instance.SetMusicVolume((float)value);
    }

    private void OnSFXVolumeChanged(double value)
    {
        GlobalSettings.Instance.SetSFXVolume((float)value);
    }
}