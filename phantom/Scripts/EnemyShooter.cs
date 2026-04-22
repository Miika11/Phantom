using Godot;

public partial class EnemyShooter : Node2D
{
    [Export] public PackedScene BulletScene;
    [Export] public SpriteFrames EnemySpriteFrames;
    [Export] public float DetectionRadius = 200f;

    private Timer _timer;
    private Marker2D _muzzleRight;
    private Marker2D _muzzleLeft;
    private Marker2D _muzzleSingle;
    private AnimatedSprite2D _sprite;

    private enum State { Dormant, Spawning, Active }
    private State _state = State.Dormant;
    private bool _detectionEnabled = false;

    public override void _Ready()
    {
        _muzzleRight  = HasNode("MuzzleRight")  ? GetNode<Marker2D>("MuzzleRight")  : null;
        _muzzleLeft   = HasNode("MuzzleLeft")   ? GetNode<Marker2D>("MuzzleLeft")   : null;
        _muzzleSingle = HasNode("Muzzle")       ? GetNode<Marker2D>("Muzzle")       : null;
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += ShootBullet;
        _sprite.AnimationFinished += OnAnimationFinished;

        if (EnemySpriteFrames != null)
            _sprite.SpriteFrames = EnemySpriteFrames;

        if (_sprite.SpriteFrames != null && _sprite.SpriteFrames.HasAnimation("lamp"))
        {
            _sprite.Play("lamp");
            var delayTimer = GetTree().CreateTimer(1.0f);
            delayTimer.Timeout += () => _detectionEnabled = true;
        }
        else
        {
            _state = State.Active;
            _sprite.Play("idle");
            _timer.Start();
        }
    }

    public override void _Process(double delta)
    {
        if (_state != State.Dormant || !_detectionEnabled) return;

        var players = GetTree().GetNodesInGroup("player");
        if (players.Count == 0) return;

        var player = players[0] as Node2D;
        if (player == null) return;

        if (GlobalPosition.DistanceTo(player.GlobalPosition) <= DetectionRadius)
            TriggerSpawn();
    }

    private void TriggerSpawn()
    {
        _state = State.Spawning;
        _sprite.Play("spawn");
    }

    private void OnAnimationFinished()
    {
        if (_state != State.Spawning) return;

        _state = State.Active;
        _sprite.Play("idle");
        _timer.Start();
    }

    private void ShootBullet()
    {
        if (_state != State.Active) return;

        _sprite.Play("shoot");

        Vector2 randomDirection = new Vector2(
            (float)GD.RandRange(-1.0, 1.0),
            (float)GD.RandRange(-1.0, 1.0)
        ).Normalized();

        if (_muzzleSingle != null) SpawnBullet(_muzzleSingle, randomDirection);
        if (_muzzleRight  != null) SpawnBullet(_muzzleRight,  randomDirection);
        if (_muzzleLeft   != null) SpawnBullet(_muzzleLeft,   randomDirection);

        var returnTimer = GetTree().CreateTimer(0.4f);
        returnTimer.Timeout += () =>
        {
            if (_state == State.Active) _sprite.Play("idle");
        };
    }

    private void SpawnBullet(Marker2D muzzle, Vector2 direction)
    {
        var bullet = BulletScene.Instantiate<Bullet>();
        GetParent().AddChild(bullet);
        bullet.GlobalPosition = muzzle.GlobalPosition;
        bullet.SetDirection(direction);
    }
}