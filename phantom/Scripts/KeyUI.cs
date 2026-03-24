using Godot;

public partial class KeyUI : HBoxContainer
{
    private Label _label; // Reference to the label that shows key amount

    public override void _Ready()
    {
        
        _label = GetNode<Label>("KeyLabel"); // gets the label node from the scene
        // GD.Print("_label: ", _label); // for debugging
    }

    public void UpdateKeys(int amount)
    {
        if (_label != null) 
            _label.Text = "x" + amount; // updates how many keys player has collected
    }
}