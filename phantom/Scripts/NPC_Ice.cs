using Godot;
using System.Collections.Generic;

public partial class NPC_Ice : Node2D
{
    private Area2D _area;
    [Export] public NodePath DialogueUIPath;
    private DialogueUI _dialogueUI;
    private bool _used = false;

    private List<Question> _questions = new List<Question>()
    {
        new Question(
            "Tällä hetkellä kaipaan eniten...",
            new[] {
                "Tilaa pohtia, kuka minä olen ja mitä oikeasti haluan.",
                "Sparrausta ja suunnitelmaa, jonka avulla saavutan päämääräni.",
                "Konkreettisia askelia ja rohkaisua kokeilla jotain omaa."
            }),

        new Question(
            "Minulle on tällä hetkellä haastavaa...",
            new[] {
                "Tietää, mikä olisi \"se oikea suunta\" tai valinta.",
                "Saada asiat etenemään järjestelmällisesti.",
                "Uskoa omaan ideaani ja löytää tapa toteuttaa sitä."
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