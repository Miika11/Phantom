using Godot;

public partial class CharacterController2 : CharacterBody2D
{
    [Export] public float Speed = 100.0f;

    private Vector2 _Movement = Vector2.Zero;
    private AnimatedSprite2D _player;
    [Export] public HeartsUI HeartsUI;

    [Export] public int MaxHealth = 3;

    private int _currentHealth;

    public override void _Ready()
    {
        _player = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        
        _currentHealth = MaxHealth;
        HeartsUI?.UpdateHearts(_currentHealth);

        GD.Print("Elämät: " + _currentHealth);
    }

    public override void _PhysicsProcess(double delta)
    {
        _Movement = Input.GetVector(
            ConfigInput.InputLeft,
            ConfigInput.InputRight,
            ConfigInput.InputForward,
            ConfigInput.InputBackward
        );

        Vector2 velocity;

        if (!Mathf.IsZeroApprox(_Movement.Length()))
        {
            velocity = _Movement.Normalized() * Speed;
        }
        else
        {
            velocity = Vector2.Zero;
        }

        Velocity = velocity;
        MoveAndSlide();
        UpdateAnimation(_Movement);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (direction == Vector2.Zero)
        {
            _player.Play(ConfigAnimation.Idle);
        }
        else if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            if (direction.X > 0)
                _player.Play(ConfigAnimation.MoveRight);
            else
                _player.Play(ConfigAnimation.MoveLeft);
        }
        else
        {
            if (direction.Y > 0)
                _player.Play(ConfigAnimation.MoveDown);
            else
                _player.Play(ConfigAnimation.MoveUp);
        }
    }

    
    public void TakeDamage(int damage)
{
    _currentHealth -= damage;

    if (_currentHealth < 0)
        _currentHealth = 0;

    GD.Print("Elämät: " + _currentHealth);

    
    HeartsUI?.UpdateHearts(_currentHealth);

    if (_currentHealth <= 0)
    {
        Die();
    }
}

    // Scene alkaa alusta kuollessa
    private void Die()
    {
        GD.Print("Pelaaja kuoli!");

        GetTree().CallDeferred("reload_current_scene");
    }
}