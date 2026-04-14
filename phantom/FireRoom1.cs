using Godot;

public partial class FireRoom1 : Node2D
{
    public override void _Ready()
    {
        var musicManager = GetNode<MusicManager>("/root/MusicManager");
        musicManager.PlayMusic("res://audio/fire_level.ogg");
    }
}