using Godot;

public partial class MagmaDemon : Area2D
{
    [Export] public float ChaseSpeed = 55f;
    [Export] public float DetectRange = 150f;
    [Export] public float ExplosionRange = 20f;
    [Export] public int Damage = 1;
	[Export] public float RemainsDuration = 3f;

    private enum State { Idle, Chase, Exploding, Remains }
    private State _state = State.Idle;

    private AnimatedSprite2D _sprite;
    private Node2D _player;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.AnimationFinished += OnAnimationFinished;
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        _sprite.Play("idle");
    }

    public override void _PhysicsProcess(double delta)
    {
        float distToPlayer = _player != null
            ? GlobalPosition.DistanceTo(_player.GlobalPosition)
            : float.MaxValue;

        switch (_state)
        {
            case State.Idle:
                if (distToPlayer < DetectRange)
                {
                    _state = State.Chase;
                }
                break;

            case State.Chase:
                if (distToPlayer < ExplosionRange)
                {
                    TriggerExplosion();
                    break;
                }
                if (distToPlayer >= DetectRange)
                {
                    _state = State.Idle;
                    _sprite.Play("idle");
                    break;
                }
                DoChase(delta);
                break;

            case State.Exploding:
            case State.Remains:
                break;
        }
    }

    private void DoChase(double delta)
    {
        Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
        GlobalPosition += direction * ChaseSpeed * (float)delta;
        _sprite.FlipH = direction.X > 0;

        string walkAnim = (Time.GetTicksMsec() % 400 < 200) ? "walk1" : "walk2";
        _sprite.Play(walkAnim);
    }

    private void TriggerExplosion()
    {
        _state = State.Exploding;
        _sprite.Play("explosion");
    }

    private async void OnAnimationFinished()
{
    if (_sprite.Animation == "explosion")
    {
        if (_player != null && GlobalPosition.DistanceTo(_player.GlobalPosition) < ExplosionRange * 1.5f)
        {
            if (_player is CharacterController2 player)
                player.TakeDamage(Damage);
        }

        _state = State.Remains;
        _sprite.Play("remains");

        await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);

        QueueFree();
    	}
	}
}