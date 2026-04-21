using Godot;

public partial class MusicManager : Node
{
    private AudioStreamPlayer _player;
    private string _currentTrack = "";

    public override void _Ready()
    {
        _player = GetNodeOrNull<AudioStreamPlayer>("AudioStreamPlayer");
        _player.Bus = "Music";
    }

    public void PlayMusic(string trackPath)
    {
        if (_currentTrack == trackPath)
            return;

        _currentTrack = trackPath;
        _player.Stream = ResourceLoader.Load<AudioStream>(trackPath);
        _player.Play();
    }

    public void StopMusic()
    {
        _player.Stop();
        _currentTrack = "";
    }
}