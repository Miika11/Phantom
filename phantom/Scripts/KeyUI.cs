using Godot;

public partial class KeyUI : HBoxContainer
{
    private Label _label;

    public override void _Ready()
    {
        _label = GetNode<Label>("KeyLabel");
    }

    public void UpdateKeys(int amount)
    {
        _label.Text = "x" + amount;
    }
}