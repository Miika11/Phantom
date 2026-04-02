using System;
using Godot;

public partial class Key : Collectable
{
    [Export] private int _score = 1;

    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        
        _sprite.Play("Idle"); 
    }

    protected override void Collect(CharacterController2 characterController2)
    {
    
    characterController2.AddKey(_score);

    

        QueueFree(); 
    }
}