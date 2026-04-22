using Godot;

public partial class Trap : Area2D
{
    [Export] public int Damage = 1;
    [Export] public float DamageCooldown = 1.0f; // prevents instant multi-hit

    private bool _onCooldown = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterController2 player && !_onCooldown)
        {
            player.TakeDamage(Damage);
            _onCooldown = true;

            var timer = GetTree().CreateTimer(DamageCooldown);
            timer.Timeout += () => _onCooldown = false;
        }
    }
}