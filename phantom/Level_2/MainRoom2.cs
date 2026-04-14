using Godot;

public partial class MainRoom2 : Node2D
{
    public override void _Ready()
    {

        GetNode<MusicManager>("/root/MusicManager")
            .PlayMusic("res://audio/ice_level.ogg");
    }
}