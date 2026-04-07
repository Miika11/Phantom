using Godot;
using System.Collections.Generic;

public partial class NPC_Fire : Node2D
{
    private Area2D _area;
    [Export] public NodePath DialogueUIPath;
    private DialogueUI _dialogueUI;
    private bool _used = false;

    private List<Question> _questions;


    public override void _Ready()
    {
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
        _dialogueUI = GetNode<DialogueUI>(DialogueUIPath);

        _questions = new List<Question>()
        {
            new Question(
                Tr("NPC_Fire_Q1"),
                new[] {
                    Tr("NPC_Fire_Q1_A1"),
                    Tr("NPC_Fire_Q1_A2"),
                    Tr("NPC_Fire_Q1_A3")
                }),

            new Question(
                Tr("NPC_Fire_Q2"),
                new[] {
                    Tr("NPC_Fire_Q2_A1"),
                    Tr("NPC_Fire_Q2_A2"),
                    Tr("NPC_Fire_Q2_A3")
                })
        };
    }

    private void OnBodyEntered(Node2D body)
    {
        if (!_used && body is CharacterBody2D)
        {
            _used = true;
            _dialogueUI.StartDialogue(_questions);
        }
    }
}