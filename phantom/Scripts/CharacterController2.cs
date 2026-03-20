using Godot;

public partial class CharacterController2 : CharacterBody2D
{
    [Export] public float Speed = 100.0f;

    private Vector2 _Movement = Vector2.Zero;
    private AnimatedSprite2D _player;

    [Export] public HeartsUI HeartsUI;
    [Export] public KeyUI KeyUI;

    private int _keys = 0;

    [Export] public int MaxHealth = 3;
    private int _currentHealth;

    public override void _Ready()
    {
        _player = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        _currentHealth = MaxHealth;
        HeartsUI?.UpdateHearts(_currentHealth);

        KeyUI?.CallDeferred("UpdateKeys", _keys);

        GD.Print("Elämät: " + _currentHealth);
    }

    public void AddKey(int amount = 1)
    {
        _keys += amount;
        GD.Print("Keys: " + _keys);
        KeyUI?.UpdateKeys(_keys);
    }

    public override void _PhysicsProcess(double delta)
    {
        _Movement = Input.GetVector(
            ConfigInput.InputLeft,
            ConfigInput.InputRight,
            ConfigInput.InputForward,
            ConfigInput.InputBackward
        );

        Vector2 velocity = _Movement.Length() > 0 ? _Movement.Normalized() * Speed : Vector2.Zero;

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
            _player.Play(direction.X > 0 ? ConfigAnimation.MoveRight : ConfigAnimation.MoveLeft);
        }
        else
        {
            _player.Play(direction.Y > 0 ? ConfigAnimation.MoveDown : ConfigAnimation.MoveUp);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        GD.Print("Elämät: " + _currentHealth);

        HeartsUI?.UpdateHearts(_currentHealth);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        GD.Print("Pelaaja kuoli!");
        GetTree().CallDeferred("reload_current_scene");
    }
}