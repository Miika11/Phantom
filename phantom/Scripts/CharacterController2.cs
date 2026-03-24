using Godot;

public partial class CharacterController2 : CharacterBody2D
{
    [Export] public float Speed = 100.0f;

    private Vector2 _Movement = Vector2.Zero;
    private AnimatedSprite2D _player;

    private HeartsUI HeartsUI;
    private KeyUI KeyUI;

    private int _keys = 0;

    [Export] public int MaxHealth = 3;
    private int _currentHealth;

    public override void _Ready()
    {
        _player = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // UI from autoload to all scenes
        var uiRoot = GetNode("/root/UI");
        HeartsUI = uiRoot.GetNode<HeartsUI>("HeartsContainer");
        KeyUI = uiRoot.GetNode<KeyUI>("KeyUI");


        // Get saved values from gamemanager and update ui to show correct values
        _currentHealth = GameManager.Instance.CurrentHealth;
        _keys = GameManager.Instance.Keys;
        HeartsUI.UpdateHearts(_currentHealth);
        KeyUI.UpdateKeys(_keys);

        GD.Print("Elämät: " + _currentHealth); 

        // Move player to spawnpoint
        Node spawnPoint = GetTree().CurrentScene.FindChild(GameManager.Instance.SpawnPoint);
        if (spawnPoint is Marker2D marker)
        {
            GlobalPosition = marker.GlobalPosition;
        }
    }

    public void AddKey(int amount = 1)
    {
        // Add key to player, save it to gamemanager and update ui
        _keys++;
        GameManager.Instance.Keys = _keys;
        KeyUI.UpdateKeys(_keys);
    }

    public override void _PhysicsProcess(double delta)
    {
        // WASD Movement
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
        _currentHealth -= damage; // reduce health
        _currentHealth = Mathf.Max(_currentHealth, 0); // So player health doesnt go below 0
        GameManager.Instance.CurrentHealth = _currentHealth; // save health to gamemanager
        HeartsUI.UpdateHearts(_currentHealth); // UI health update

        GD.Print("Elämät: " + _currentHealth);

        HeartsUI?.UpdateHearts(_currentHealth);

        if (_currentHealth <= 0) // if health goes to 0 player dies
            Die();
    }

    private void Die()
    {
        GD.Print("Pelaaja kuoli!");

        GameManager.Instance.CurrentHealth = GameManager.Instance.MaxHealth; // Reset health to full before reloading

        GetTree().CallDeferred("reload_current_scene"); // Reload scene (restart level)
    }
}