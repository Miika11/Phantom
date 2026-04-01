using Godot;

public partial class CharacterController2 : CharacterBody2D
{
    [Export] public float Speed = 100.0f;
    [Export] public NodePath JoystickPath;

    private Vector2 _Movement = Vector2.Zero;
    private AnimatedSprite2D _player;
    private Joystick _joystick;

    private HeartsUI HeartsUI;
    private KeyUI KeyUI;

    private int _keys = 0;

    [Export] public int MaxHealth = 3;
    private int _currentHealth;

    
    [Export] public float BoostMultiplier = 1.8f;
    [Export] public float MaxBoost = 100f;
    [Export] public float BoostDrain = 40f;
    [Export] public float BoostRecharge = 25f;
    [Export] public float RechargeDelay = 1.0f;

    private float _boost;
    private bool _isBoosting = false;
    private float _rechargeTimer = 0f;

    private TextureProgressBar BoostBar;
    private TextureButton BoostButton;

    public override void _Ready()
    {
        _player = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _joystick = GetNode<Joystick>(JoystickPath);

        // UI from autoload
        var uiRoot = GetNode("/root/UI");

        HeartsUI = uiRoot.GetNode<HeartsUI>("HeartsContainer");
        KeyUI = uiRoot.GetNode<KeyUI>("KeyUI");
        BoostBar = uiRoot.GetNode<TextureProgressBar>("BoostBar");
        BoostButton = uiRoot.GetNode<TextureButton>("BoostButton");

        BoostButton.ButtonDown += StartBoost;
        BoostButton.ButtonUp += StopBoost;

        // Load values from GameManager
        _currentHealth = GameManager.Instance.CurrentHealth;
        _keys = GameManager.Instance.Keys;

        GameManager.Instance.KeysOnRoomEnter = _keys;

        HeartsUI.UpdateHearts(_currentHealth);
        KeyUI.UpdateKeys(_keys);

        
        _boost = MaxBoost;
        BoostBar.MaxValue = MaxBoost;
        BoostBar.Value = _boost;

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
        _keys += amount;
        GameManager.Instance.Keys = _keys;
        KeyUI.UpdateKeys(_keys);
    }

            public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        _Movement = _joystick.GetDirection();

        // Mobile boost input
        _isBoosting = BoostButton.ButtonPressed;

        HandleBoost(dt);

        float currentSpeed = Speed;

        if (_isBoosting && _boost > 0 && _Movement != Vector2.Zero)
        {
            currentSpeed *= BoostMultiplier;
        }

        Velocity = _Movement.Length() > 0 ? _Movement.Normalized() * currentSpeed : Vector2.Zero;

        MoveAndSlide();

        UpdateAnimation(_Movement);
    }

    private void HandleBoost(float delta)
    {
        if (_isBoosting && _boost > 0 && _Movement != Vector2.Zero)
        {
            _boost -= BoostDrain * delta;
            _rechargeTimer = RechargeDelay;
        }
        else
        {
            if (_rechargeTimer > 0)
            {
                _rechargeTimer -= delta;
            }
            else
            {
                _boost += BoostRecharge * delta;
            }
        }

        _boost = Mathf.Clamp(_boost, 0, MaxBoost);

        if (_boost <= 0)
            _isBoosting = false;

        BoostBar.Value = _boost;


        if (_boost < MaxBoost * 0.2f)
            BoostBar.Modulate = Colors.Red;
        else
            BoostBar.Modulate = Colors.White;
    }

    public void StartBoost()
    {
        _isBoosting = true;
    }

    public void StopBoost()
    {
        _isBoosting = false;
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

        GameManager.Instance.CurrentHealth = _currentHealth;
        HeartsUI.UpdateHearts(_currentHealth);

        GD.Print("Elämät: " + _currentHealth);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        GD.Print("Pelaaja kuoli!");

        GameManager.Instance.CurrentHealth = GameManager.Instance.MaxHealth;
        GameManager.Instance.Keys = GameManager.Instance.KeysOnRoomEnter;

        GetTree().CallDeferred("reload_current_scene");
    }
}