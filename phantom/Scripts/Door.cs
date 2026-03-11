using Godot;
using System;

public partial class Door : Area2D
{
	[Export]
	public string TargetScene {get; set;} = "";
    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
    }
	private void OnBodyEntered(Node2D body)
	{

		if (body is CharacterBody2D && TargetScene != "")
		{
			GetTree().ChangeSceneToFile(TargetScene);
		}
	}
}
