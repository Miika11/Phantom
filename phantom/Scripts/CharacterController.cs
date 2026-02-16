using Godot;
using System;

public partial class CharacterController : CharacterBody2D
{
	public const float Speed = 500.0f;
	private Vector2 _Movement = Vector2.Zero;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_Movement = Input.GetVector(InputConfig.InputLeft, InputConfig.InputRight, InputConfig.InputForward, InputConfig.InputBackward);

		GD.Print("Movement: " + _Movement);
	}

    public override void _PhysicsProcess(double delta)
    {
		Vector2 velocity;


        if(!Mathf.IsZeroApprox(_Movement.Length()))
		{
			velocity = _Movement * Speed;
		}
		else
		{
			velocity = Vector2.Zero;
		}

		Velocity = velocity;
		MoveAndSlide();

    }
}
