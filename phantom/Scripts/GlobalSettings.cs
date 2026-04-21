using Godot;

public partial class GlobalSettings : Node
{
    public static GlobalSettings Instance { get; private set; }
    private string _currentLocale = "en";

    private float _musicVolume = -6f;
    private float _sfxVolume = -6f;

    [Signal] public delegate void LocaleChangedEventHandler();

    public override void _Ready()
    {
        Instance = this;
        ApplyMusicVolume(_musicVolume);
        ApplySFXVolume(_sfxVolume);
    }

    public void SetLocale(string locale)
    {
        _currentLocale = locale;
        TranslationServer.SetLocale(locale);
        EmitSignal(SignalName.LocaleChanged);
    }

    public string GetLocale() => _currentLocale;

    public float GetMusicVolume() => _musicVolume;
    public float GetSFXVolume() => _sfxVolume;

    public void SetMusicVolume(float volumeDb)
    {
        _musicVolume = volumeDb;
        ApplyMusicVolume(volumeDb);
    }

    public void SetSFXVolume(float volumeDb)
    {
        _sfxVolume = volumeDb;
        ApplySFXVolume(volumeDb);
    }

    private void ApplyMusicVolume(float volumeDb)
    {
        int busIndex = AudioServer.GetBusIndex("Music");
        AudioServer.SetBusVolumeDb(busIndex, volumeDb);
    }

    private void ApplySFXVolume(float volumeDb)
    {
        int busIndex = AudioServer.GetBusIndex("SFX");
        AudioServer.SetBusVolumeDb(busIndex, volumeDb);
    }
}