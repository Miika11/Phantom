using Godot;
using System;

public partial class MainMenu : Node2D
{

	public override void _Ready()
	{
		GetNode<Button>("Control/PLAY").Pressed += OnPlayPressed;
		GetNode<Button>("Control/QUIT").Pressed += OnQuitPressed;
	}

	private void OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://intro.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
	public override void _Process(double delta)
	{
	}
}
