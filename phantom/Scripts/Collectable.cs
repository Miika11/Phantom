using Godot;
using System;

public partial class Collectable : Area2D
{
	private bool _isCollected = false;

	public bool IsCollected
	{
		get { return _isCollected; }
	}

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
    }

	private void OnBodyEntered(Node2D Body)
	{
		if (Body is CharacterController2 characterController2)
		{
		_isCollected = true;
		Collect(characterController2);

		QueueFree();
		}
	}
	protected virtual void Collect(CharacterController2 characterController2)
	{

	}


}


