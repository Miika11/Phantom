using Godot;

public partial class Enemy : Area2D
{
    [Export] public float Speed = 80f;
    [Export] public float WaitTime = 0.5f;

    private Vector2[] _waypoints;
    private int _currentWaypoint = 0;
    private bool _waiting = false;
    private bool _hasDamaged = false;
    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");  

        var waypointList = new System.Collections.Generic.List<Vector2>();
        foreach (Node child in GetChildren())
        {
            if (child is Marker2D marker)
                waypointList.Add(marker.GlobalPosition);
        }
        _waypoints = waypointList.ToArray();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_waypoints.Length == 0 || _waiting) return;

        Vector2 target = _waypoints[_currentWaypoint];
        Vector2 direction = (target - GlobalPosition).Normalized();
        float distanceToTarget = GlobalPosition.DistanceTo(target);

        if (distanceToTarget < 4f)
        {
            GlobalPosition = target;
            _waiting = true;
            UpdateAnimation(Vector2.Zero);  // idle when waiting

            var timer = GetTree().CreateTimer(WaitTime);
            timer.Timeout += () =>
            {
                _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
                _waiting = false;
            };
        }
        else
        {
            GlobalPosition += direction * Speed * (float)delta;
            UpdateAnimation(direction);  //moving animation
        }
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (direction == Vector2.Zero)
        {
            
            string current = _sprite.Animation;
            if (current == "move_left" || current == "idle_left")
                _sprite.Play("idle_left");
            else
                _sprite.Play("idle_right");
        }
        else if (direction.X < 0)
        {
            _sprite.Play("move_left");
        }
        else
        {
            _sprite.Play("move_right");
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterController2 player && !_hasDamaged)
        {
            _hasDamaged = true;
            player.TakeDamage(1);
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is CharacterController2)
            _hasDamaged = false;
    }
}