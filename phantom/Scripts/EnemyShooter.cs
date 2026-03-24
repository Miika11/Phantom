using Godot;

public partial class EnemyShooter : Node2D
{
    [Export]
    public PackedScene BulletScene;

    private Timer _timer; // Timer that controls how often enemy shoots
    private Marker2D _muzzleRight; // Right hand
    private Marker2D _muzzleLeft;  // Left hand
    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        _muzzleRight = GetNode<Marker2D>("MuzzleRight");
        _muzzleLeft = GetNode<Marker2D>("MuzzleLeft");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += ShootBullet;
        _timer.Start();
    }

    // Creates and shoots the bullet
    private void ShootBullet()
    {
        _sprite.Play("shoot");

        // Same random direction for both bullets
        Vector2 randomDirection = new Vector2(
            (float)GD.RandRange(-1.0, 1.0),
            (float)GD.RandRange(-1.0, 1.0)
        ).Normalized();

        SpawnBullet(_muzzleRight, randomDirection);
        SpawnBullet(_muzzleLeft, randomDirection);

        var returnTimer = GetTree().CreateTimer(0.4f);
        returnTimer.Timeout += () => _sprite.Play("idle");
    }

    private void SpawnBullet(Marker2D muzzle, Vector2 direction)
    {
        var bullet = BulletScene.Instantiate<Bullet>();
        GetParent().AddChild(bullet);
        bullet.GlobalPosition = muzzle.GlobalPosition;
        bullet.SetDirection(direction);
    }
}