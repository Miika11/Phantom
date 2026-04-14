using Godot;

public partial class MainRoom3 : Node2D
{
    public override void _Ready()
    {
        GetNode<MusicManager>("/root/MusicManager")
            .PlayMusic("res://audio/desert_level.ogg");
    }
}