using Godot;
using System;

public partial class Settings : CanvasLayer
{
	[Export] public Button _mainmenu;
	public override void _Ready()
	{
		_mainmenu.Pressed += OnMenuPressed;
	}

	private void OnMenuPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
	public override void _Process(double delta)
	{
	}
}
