using Godot;

public partial class Bullet : Area2D
{
    [Export]
    public float Speed = 200f;

    [Export]
    public int Damage = 1;

    private Vector2 _direction;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * Speed * (float)delta;
    }

    // luodin suunta
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.Normalized();
        Rotation = _direction.Angle(); // Bulletti image menee nyt ampumis suuntaan
    }

    // luodin osuminen pelaajaan
    private void OnBodyEntered(Node body)
    {
        if (body is CharacterController2 player)
        {
            player.TakeDamage(Damage);
            QueueFree();
        }
    }
}