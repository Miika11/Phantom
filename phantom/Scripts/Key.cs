using System;
using Godot;

public partial class Key : Collectable
{
    [Export] private int _score = 1;

    private AnimatedSprite2D _sprite;
     private SFXManager _sfx;

    public override void _Ready()
    {
        _sfx = GetNode<SFXManager>("/root/SFXManager");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        
        _sprite.Play("Idle"); 
    }

    protected override void Collect(CharacterController2 characterController2)
    {
    
    characterController2.AddKey(_score);
    _sfx.PlayKey();
        QueueFree(); 
    }
}