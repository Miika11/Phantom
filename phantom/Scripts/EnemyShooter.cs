using Godot;

public partial class EnemyShooter : Node2D
{
    [Export] public PackedScene BulletScene;
    [Export] public SpriteFrames EnemySpriteFrames;

    private Timer _timer; // Timer that controls how often enemy shoots
    private Marker2D _muzzleRight; // Right hand
    private Marker2D _muzzleLeft;  // Left hand
    private Marker2D _muzzleSingle; // For single-muzzle enemies
    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        _muzzleRight  = HasNode("MuzzleRight")  ? GetNode<Marker2D>("MuzzleRight")  : null;
        _muzzleLeft   = HasNode("MuzzleLeft")   ? GetNode<Marker2D>("MuzzleLeft")   : null;
        _muzzleSingle = HasNode("Muzzle")       ? GetNode<Marker2D>("Muzzle")       : null;
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += ShootBullet;
        _timer.Start();

        if (EnemySpriteFrames != null)
            _sprite.SpriteFrames = EnemySpriteFrames;
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

            // Fire from whichever muzzles exist
            if (_muzzleSingle != null) SpawnBullet(_muzzleSingle, randomDirection);
            if (_muzzleRight  != null) SpawnBullet(_muzzleRight,  randomDirection);
            if (_muzzleLeft   != null) SpawnBullet(_muzzleLeft,   randomDirection);

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