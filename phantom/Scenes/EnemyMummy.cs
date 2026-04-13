using Godot;

public partial class EnemyMummy : Area2D
{
    [Export] public float Speed = 30f;
    [Export] public float ChaseSpeed = 55f;
    [Export] public float DetectRange = 150f;
    [Export] public int Damage = 1;
    [Export] public float DamageCooldown = 1.0f; // Prevents dealing damage every single frame

    private enum State { Patrol, Chase }
    private State _state = State.Patrol;

    private Vector2[] _waypoints;
    private int _currentWaypoint = 0;
    private bool _waiting = false;
    private bool _onCooldown = false;
    private AnimatedSprite2D _sprite;
    private Node2D _player;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        var waypointList = new System.Collections.Generic.List<Vector2>();
        foreach (Node child in GetChildren())
        {
            if (child is Marker2D marker)
                waypointList.Add(marker.GlobalPosition);
        }
        _waypoints = waypointList.ToArray();

        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        float distToPlayer = _player != null
            ? GlobalPosition.DistanceTo(_player.GlobalPosition)
            : float.MaxValue;

        switch (_state)
        {
            case State.Patrol:
                if (distToPlayer < DetectRange)
                {
                    _waiting = false;
                    _state = State.Chase;
                    break;
                }
                DoPatrol(delta);
                break;

            case State.Chase:
                if (distToPlayer >= DetectRange)
                {
                    _state = State.Patrol;
                    break;
                }
                DoChase(delta);
                break;
        }
    }

    private void DoPatrol(double delta)
    {
        if (_waypoints.Length == 0 || _waiting) return;

        Vector2 target = _waypoints[_currentWaypoint];
        Vector2 direction = (target - GlobalPosition).Normalized();

        if (GlobalPosition.DistanceTo(target) < 4f)
        {
            GlobalPosition = target;
            _waiting = true;
            _sprite.Play("idle");

            var timer = GetTree().CreateTimer(1.0f);
            timer.Timeout += () =>
            {
                _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
                _waiting = false;
            };
        }
        else
        {
            GlobalPosition += direction * Speed * (float)delta;
            _sprite.FlipH = direction.X > 0;
            _sprite.Play("walk");
        }
    }

    private void DoChase(double delta)
    {
        Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
        GlobalPosition += direction * ChaseSpeed * (float)delta;
        _sprite.FlipH = direction.X > 0;
        _sprite.Play("walk");
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