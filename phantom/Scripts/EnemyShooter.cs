using Godot;

public partial class EnemyShooter : Node2D
{
    [Export]
    public PackedScene BulletScene;

    // Timeri joka ampuu tietyn välin jälkeen
    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += ShootBullet;
        _timer.Start();
    }

    // metodi joka luo ja ampuu bulletin random suuntaan
    private void ShootBullet()
    {
        var bullet = BulletScene.Instantiate<Bullet>();

        GetParent().AddChild(bullet);

        bullet.GlobalPosition = GlobalPosition;

		Vector2 randomDirection = new Vector2(
			(float)GD.RandRange(-1.0, 1.0),
			(float)GD.RandRange(-1.0, 1.0)
		).Normalized();

        bullet.SetDirection(randomDirection);
    }
}