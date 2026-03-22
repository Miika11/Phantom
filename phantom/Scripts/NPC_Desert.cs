using Godot;
using System.Collections.Generic;

public partial class NPC_Desert : Node2D
{
    private Area2D _area;
    [Export] public NodePath DialogueUIPath;
    private DialogueUI _dialogueUI;
    private bool _used = false;

    private List<Question> _questions = new List<Question>()
    {
        new Question(
            "Kun mietin elämääni tällä hetkellä...",
            new[] {
                "En ole ihan varma, mihin suuntaan olen menossa.",
                "Tiedän, mitä haluan – mutta kaipaan rakennetta ja fokusta.",
                "Minulla on idea, jonka haluaisin muuttaa joksikin konkreettiseksi."
            }),

        new Question(
            "Tärkein syyni hakeutua mentorointiin on...",
            new[] {
                "Itsetuntemuksen ja oman suunnan selkeyttäminen.",
                "Eteneminen kohti tavoitteita mahdollisimman tehokkaasti.",
                "Halu kehittää omaa juttua, ehkä jopa liiketoimintaa."
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