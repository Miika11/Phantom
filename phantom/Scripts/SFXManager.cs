using Godot;

public partial class SFXManager : Node
{
    private AudioStreamPlayer[] _pool;
    private int _poolIndex = 0;
    private const int PoolSize = 4;

    private static readonly AudioStream Click = GD.Load<AudioStream>("res://audio/click.wav");
    private static readonly AudioStream Hurt = GD.Load<AudioStream>("res://audio/hurt.wav");
    private static readonly AudioStream Boost = GD.Load<AudioStream>("res://audio/boost.wav");

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _pool = new AudioStreamPlayer[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            _pool[i] = new AudioStreamPlayer();
            _pool[i].Bus = "SFX";
            _pool[i].ProcessMode = ProcessModeEnum.Always;
            AddChild(_pool[i]);
        }
    }

    public void PlayClick() => Play(Click, -5f);
    public void PlayHurt() => Play(Hurt, 0f);
    public void PlayBoost() => Play(Boost, -3f);

    public void SetSFXVolume(float volumeDb)
    {
        int busIndex = AudioServer.GetBusIndex("SFX");
        AudioServer.SetBusVolumeDb(busIndex, volumeDb);
    }

    private void Play(AudioStream stream, float volumeDb = 0f)
    {
        if (stream == null)
        {
            GD.PrintErr("SFXManager: stream is null");
            return;
        }
        _pool[_poolIndex].VolumeDb = volumeDb;
        _pool[_poolIndex].Stream = stream;
        _pool[_poolIndex].Play();
        _poolIndex = (_poolIndex + 1) % PoolSize;
    }
}