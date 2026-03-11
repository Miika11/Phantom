using Godot;

public partial class Enemy : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterController2 player)
        {
            player.TakeDamage(1);
        }
    }
}