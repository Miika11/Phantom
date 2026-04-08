using Godot;

public partial class ResultNPC : Node2D
{
    private Area2D _area;

    [Export] public NodePath DialogueUIPath;
    [Export] public Texture2D DialogueBackground;
    private DialogueUI _dialogueUI;

    public override void _Ready()
    {
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;

        _dialogueUI = GetNode<DialogueUI>(DialogueUIPath);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is CharacterBody2D)
        {
            if (GameManager.Instance.PlayerAnswers.Count < 6)
            {
                GD.Print("Vastaa kaikkiin kysymyksiin ensin!");
                return;
            }

            string result = GameManager.Instance.GetResult();


            _dialogueUI.SetBackground(DialogueBackground);


            _dialogueUI.ShowFinalResult(result);
        }
    }
}