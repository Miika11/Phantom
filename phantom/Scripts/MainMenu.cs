using Godot;
using System;

public partial class MainMenu : Node2D
{

	public override void _Ready()
	{
		GetTree().CallGroup("GameHUD", "hide"); // hide ui when menu starts
		GetNode<Button>("Control/PLAY").Pressed += OnPlayPressed;
		GetNode<Button>("Control/QUIT").Pressed += OnQuitPressed;
		GetNode<Button>("Control/SUOMI").Pressed += () => TranslationServer.SetLocale("fi");
		GetNode<Button>("Control/ENGLISH").Pressed += () => TranslationServer.SetLocale("en");
	}

	private void OnPlayPressed()
	{
		GetTree().CallGroup("GameHUD", "show"); // show ui in next scene
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
