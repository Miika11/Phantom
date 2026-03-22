using Godot;
using System;

public partial class Door : Area2D
{
    [Export]
    public string TargetScene { get; set; } = "";

    [Export]
    public string TargetSpawnPoint { get; set; } = "StartSpawn";

    private bool _opened = false;

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
        if (body is CharacterBody2D && TargetScene != "" && !_opened)
        {
            _opened = true;
            GameManager.Instance.SpawnPoint = TargetSpawnPoint;

            // Defer scene change
            CallDeferred(nameof(ChangeScene));
        }
    }

    private void ChangeScene()
    {
        GetTree().ChangeSceneToFile(TargetScene);
    }
}