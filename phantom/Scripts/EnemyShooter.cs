using Godot;

public partial class EnemyShooter : Node2D
{
    [Export]
    public PackedScene BulletScene;

    private Timer _timer; // Timer that controls how often enemy shoots

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += ShootBullet;
        _timer.Start();
    }

    // Creates and shoots the bullet
    private void ShootBullet()
    {
        var bullet = BulletScene.Instantiate<Bullet>();

        GetParent().AddChild(bullet);

        bullet.GlobalPosition = GlobalPosition;

		Vector2 randomDirection = new Vector2( // Bullets go in random direction
			(float)GD.RandRange(-1.0, 1.0),
			(float)GD.RandRange(-1.0, 1.0)
		).Normalized();

        bullet.SetDirection(randomDirection);
    }
}