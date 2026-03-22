using Godot;
using System.Collections.Generic;

public partial class NPC_Fire : Node2D
{
    private Area2D _area;
    [Export] public NodePath DialogueUIPath;
    private DialogueUI _dialogueUI;
    private bool _used = false;

    private List<Question> _questions = new List<Question>()
    {
        new Question(
            "Jos saisin valita yhden lahjan mentorilta...",
            new[] {
                "Syvällisiä kysymyksiä ja turvallisen tilan omien ajatusten tutkimiseen.",
                "Suunnitelman, deadlinet ja napakan tsempin.",
                "Rehellistä palautetta, bisnesnäkökulmaa ja rohkaisua kokeilla."
            }),

        new Question(
            "Millä lauseella tiivistäisit fiiliksesi?",
            new[] {
                "Olen liikkeellä, mutta en vielä tiedä, minne tie vie.",
                "Tiedän minne haluan mennä – nyt tarttis vaan mennä.",
                "Minulla on idea – mutta miten siitä tulee totta?"
            })
    };

    public override void _Ready()
    {
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
        _dialogueUI = GetNode<DialogueUI>(DialogueUIPath);
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