using Godot;

public partial class MusicManager : Node
{
    private AudioStreamPlayer _player;
    private string _currentTrack = "";

    public override void _Ready()
{
    GD.Print("✔ MusicManager _Ready() called");

    _player = GetNodeOrNull<AudioStreamPlayer>("AudioStreamPlayer");

    if (_player == null)
    {
        GD.PrintErr("❌ AudioStreamPlayer NOT FOUND");
    }
    else
    {
        GD.Print("✔ AudioStreamPlayer found");
    }
}

    public void PlayMusic(string trackPath)
    {
        // Don't restart if the same track is already playing
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