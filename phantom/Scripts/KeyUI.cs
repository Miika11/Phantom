using Godot;

public partial class KeyUI : HBoxContainer
{
    private Label _label;

    public override void _Ready()
    {
        
        _label = GetNode<Label>("KeyLabel");
        // GD.Print("_label: ", _label); // for debugging
    }

    public void UpdateKeys(int amount)
    {
        if (_label != null) 
            _label.Text = "x" + amount;
    }
}