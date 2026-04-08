using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
	[Export] public TextureButton _play;
	[Export] public TextureButton _quit;
	[Export] public TextureButton _settings;
	[Export] public CanvasLayer _settingsmenu;
	public override void _Ready()
	{
		GetTree().CallGroup("GameHUD", "hide"); // hide ui when menu starts
		_settingsmenu.Visible = false;
		_settings.Pressed += OnSettingsPressed;
		// GetNode<Button>("Control/PLAY").Pressed += OnPlayPressed;
		// GetNode<Button>("Control/QUIT").Pressed += OnQuitPressed;
		// GetNode<Button>("Control/SUOMI").Pressed += () => TranslationServer.SetLocale("fi");
		// GetNode<Button>("Control/ENGLISH").Pressed += () => TranslationServer.SetLocale("en");
        // GetNode<TextureButton>("Control/PlayButton").Pressed += OnPlayPressed;
        // GetNode<TextureButton>("Control/QuitButton").Pressed += OnQuitPressed;
		_play.Pressed += OnPlayPressed;
		_quit.Pressed += OnQuitPressed;
	}
	private void OnSettingsPressed()
	{
		_settingsmenu.Visible = true;
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
