using Godot;
using System;

public partial class Door : Area2D
{
    [Export] public string TargetScene { get; set; } = "";
    [Export] public string TargetSpawnPoint { get; set; } = "StartSpawn";

    [Export] public int KeysRequired { get; set; } = 0; // Number of keys needed to open

    [Export] public bool ConsumeKeysOnOpen { get; set; } = false; // Reset keys when opening

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
        if (_opened) return; // prevent multiple triggers

        if (body is CharacterBody2D)
        {
            int playerKeys = GameManager.Instance.Keys;

            if (playerKeys >= KeysRequired)
            {
                // Player has enough keys, open the door
                _opened = true;

                // Optionally consume keys
                if (ConsumeKeysOnOpen && KeysRequired > 0)
                {
                    GameManager.Instance.Keys -= KeysRequired;

                    // Update the UI
                    var uiRoot = GetNode("/root/UI");
                    var keyUI = uiRoot.GetNode<KeyUI>("KeyUI");
                    keyUI.UpdateKeys(GameManager.Instance.Keys);
                }

                // Set spawn point for the next scene
                GameManager.Instance.SpawnPoint = TargetSpawnPoint;

                // Defer scene change to avoid physics callback issues
                CallDeferred(nameof(ChangeScene));
            }
            else
            {
                // Not enough keys
                GD.Print($"Tarvitset {KeysRequired} avainta avataksesi oven. Sinulla on {playerKeys}.");
            }
        }
    }

    private void ChangeScene()
    {
        if (TargetScene != "")
        {
            GetTree().ChangeSceneToFile(TargetScene);
        }
    }
}