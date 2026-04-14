using Godot;

public partial class Intro : Node2D
{
    public override void _Ready()
    {

        GetNode<MusicManager>("/root/MusicManager")
            .PlayMusic("res://audio/fire_level.ogg");
    }
}