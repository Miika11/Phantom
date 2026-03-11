using Godot;
using System;


public partial class Key : Collectable
{
	[Export] private int _score = 1;

	protected override void Collect(CharacterController2 characterController2)
	{
		GD.Print($"Kerättiin avain: {_score}");
		GameManager.Instance.AddScore(_score);
	}

}
