using Godot;

public partial class EnemySnowman : Area2D
{
    [Export] public float Speed = 40f;
    [Export] public float ChaseSpeed = 70f;         // Faster when chasing
    [Export] public float DetectRange = 120f;        // Range to start chasing
    [Export] public float AttackRange = 40f;         // Range to trigger attack
    [Export] public int Damage = 1;
    [Export] public float AttackCooldown = 1.5f;

    private enum State { Patrol, Chase, Attack, Cooldown }
    private State _state = State.Patrol;

    private Vector2[] _waypoints;
    private int _currentWaypoint = 0;
    private bool _waiting = false;
    private bool _hasDamaged = false;
    private AnimatedSprite2D _sprite;
    private Node2D _player;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.AnimationFinished += OnAnimationFinished;

        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;

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
        float distToPlayer = _player != null
            ? GlobalPosition.DistanceTo(_player.GlobalPosition)
            : float.MaxValue;

        switch (_state)
        {
            case State.Patrol:
                // Transition: player entered detect range
                if (distToPlayer < DetectRange)
                {
                    _waiting = false;
                    _state = State.Chase;
                    break;
                }
                DoPatrol(delta);
                break;

            case State.Chase:
                // Transition: player escaped
                if (distToPlayer >= DetectRange)
                {
                    _state = State.Patrol;
                    break;
                }
                
                if (distToPlayer < AttackRange)
                {
                    _state = State.Attack;
                    StartAttack();
                    break;
                }
                DoChase(delta);
                break;

            case State.Attack:
            case State.Cooldown:
                
                if (_player != null)
                    _sprite.FlipH = (_player.GlobalPosition.X - GlobalPosition.X) > 0;
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

            var timer = GetTree().CreateTimer(0.5f);
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
            _sprite.Play("idle");
        }
    }

    

        private void DoChase(double delta)
    {
        Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
        GlobalPosition += direction * ChaseSpeed * (float)delta;
        _sprite.FlipH = direction.X > 0;
        _sprite.Play("charge"); // rushing toward player
    }

   
    private void StartAttack()
    {
        _hasDamaged = false;

        if (_player != null)
            _sprite.FlipH = (_player.GlobalPosition.X - GlobalPosition.X) > 0;

        _sprite.Play("charge");

        var chargeTimer = GetTree().CreateTimer(0.6f);
        chargeTimer.Timeout += () =>
        {
            if (!IsInsideTree()) return;
            if (_state == State.Attack)
                _sprite.Play("punch");
        };

        // Deal damage at the moment the punch lands
        var damageTimer = GetTree().CreateTimer(0.6f + 0.3f);
        damageTimer.Timeout += () =>
        {
            if (!IsInsideTree()) return;
            if (_state == State.Attack && !_hasDamaged && _player != null)
            {
                if (GlobalPosition.DistanceTo(_player.GlobalPosition) < AttackRange * 1.2f)
                {
                    _hasDamaged = true;
                    if (_player is CharacterController2 player)
                        player.TakeDamage(Damage);
                }
            }
        };

        var safetyTimer = GetTree().CreateTimer(0.6f + 0.8f);
        safetyTimer.Timeout += () =>
        {
            if (!IsInsideTree()) return;
            if (_state == State.Attack)
                ResetAfterAttack();
        };
    }

    private void OnAnimationFinished()
    {
        if (_sprite.Animation == "punch")
            ResetAfterAttack();
    }

    private void ResetAfterAttack()
    {
        _state = State.Cooldown;
        _sprite.Play("idle");

        var timer = GetTree().CreateTimer(AttackCooldown);
        timer.Timeout += () =>
        {
            if (!IsInsideTree()) return;
            float dist = _player != null
                ? GlobalPosition.DistanceTo(_player.GlobalPosition)
                : float.MaxValue;

            _state = dist < DetectRange ? State.Chase : State.Patrol;
        };
    }

   
    private void OnBodyEntered(Node body)
    {
        if (body is CharacterController2 player && _state == State.Attack && !_hasDamaged)
        {
            _hasDamaged = true;
            player.TakeDamage(Damage);
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is CharacterController2)
            _hasDamaged = false;
    }
}