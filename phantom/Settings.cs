using Godot;

public partial class Settings : CanvasLayer
{
    private Button _mainmenu;
    private Button _suomi;
    private Button _english;
    private SFXManager _sfx;

    private HSlider _musicSlider;
    private HSlider _sfxSlider;

    public override void _Ready()
    {
        _sfx = GetNode<SFXManager>("/root/SFXManager");

        _mainmenu = GetNode<Button>("ColorRect/BACKTOMENU");
        _suomi = GetNode<Button>("ColorRect/SUOMI");
        _english = GetNode<Button>("ColorRect/ENGLISH");

        _musicSlider = GetNode<HSlider>("ColorRect/MusicSlider");
        _sfxSlider = GetNode<HSlider>("ColorRect/SFXSlider");

        // Set sliders to current volume values
        _musicSlider.Value = GlobalSettings.Instance.GetMusicVolume();
        _sfxSlider.Value = GlobalSettings.Instance.GetSFXVolume();

        _mainmenu.Pressed += OnMenuPressed;
        _suomi.Pressed += OnSuomiPressed;
        _english.Pressed += OnEnglishPressed;
        _musicSlider.ValueChanged += OnMusicVolumeChanged;
        _sfxSlider.ValueChanged += OnSFXVolumeChanged;
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

    private void OnMusicVolumeChanged(double value)
    {
        GlobalSettings.Instance.SetMusicVolume((float)value);
    }

    private void OnSFXVolumeChanged(double value)
    {
        GlobalSettings.Instance.SetSFXVolume((float)value);
    }
}