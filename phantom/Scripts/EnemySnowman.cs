using Godot;

public partial class EnemySnowman : Area2D
{
    [Export] public float Speed = 40f;          // patrol speed
    [Export] public float DetectRange = 120f;   // How close to trigger attack
    [Export] public int Damage = 1;
    [Export] public float AttackCooldown = 1.5f;

    private Vector2[] _waypoints;
    private int _currentWaypoint = 0;
    private bool _waiting = false;
    private bool _isAttacking = false;
    private bool _hasDamaged = false;
    private bool _onCooldown = false;
    private AnimatedSprite2D _sprite;
    private Node2D _player;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.AnimationFinished += OnAnimationFinished;

        // Find player in the scene
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
        if (_isAttacking || _onCooldown) return;

        // Check if player is close enough to attack
        if (_player != null && GlobalPosition.DistanceTo(_player.GlobalPosition) < DetectRange)
        {
            StartAttack();
            return;
        }

        // Otherwise patrol normally
        if (_waypoints.Length == 0 || _waiting) return;

        Vector2 target = _waypoints[_currentWaypoint];
        Vector2 direction = (target - GlobalPosition).Normalized();
        float distanceToTarget = GlobalPosition.DistanceTo(target);

        if (distanceToTarget < 4f)
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

		private void StartAttack()
		{
			_isAttacking = true;
			_hasDamaged = false;

			if (_player != null)
				_sprite.FlipH = (_player.GlobalPosition.X - GlobalPosition.X) > 0;

			_sprite.Play("charge");

			var chargeTimer = GetTree().CreateTimer(0.6f);
			chargeTimer.Timeout += () =>
			{
				if (_isAttacking) // Only punch if still in attack state
					_sprite.Play("punch");
			};


			var safetyTimer = GetTree().CreateTimer(0.6f + 0.8f); // charge time + punch time
			safetyTimer.Timeout += () =>
			{
				if (_isAttacking)
				{
					ResetAfterAttack();
				}
			};
		}

		private void OnAnimationFinished()
		{
			if (_sprite.Animation == "punch")
				ResetAfterAttack();
		}

		private void ResetAfterAttack()
		{
			_isAttacking = false;
			_sprite.Play("idle");

			_onCooldown = true;
			var timer = GetTree().CreateTimer(AttackCooldown);
			timer.Timeout += () => _onCooldown = false;
		}

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterController2 player && _isAttacking && !_hasDamaged)
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