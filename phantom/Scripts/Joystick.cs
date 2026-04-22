using Godot;
 
public partial class Joystick : Control
{
    [Export] public float HandleLimit = 40f;
 
    private Vector2 _input = Vector2.Zero;
    private TextureRect _handle;
    private TextureRect _background;
    private int _touchIndex = -1;
 
    private Vector2 _center;
    private Vector2 _handleRestPosition;
 
    public override void _Ready()
    {
        _background = GetNode<TextureRect>("Background");
        _handle = GetNode<TextureRect>("Handle");
 
        // Center of the background circle
        _center = _background.Position + _background.Size / 2f;
 
        // Place handle centered on the background at rest
        _handleRestPosition = _center - _handle.Size / 2f;
        _handle.Position = _handleRestPosition;
    }
 
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touch)
        {
            if (touch.Pressed && _touchIndex == -1)
            {
                _touchIndex = touch.Index;
                _handle.Position = _handleRestPosition;
                _input = Vector2.Zero;
            }
            else if (!touch.Pressed && touch.Index == _touchIndex)
            {
                _touchIndex = -1;
                _input = Vector2.Zero;
                _handle.Position = _handleRestPosition;
            }
        }
        else if (@event is InputEventScreenDrag drag)
        {
            if (drag.Index != _touchIndex) return;
 
            // delta from the joystick center
            Vector2 delta = drag.Position - _center;
            if (delta.Length() > HandleLimit)
                delta = delta.Normalized() * HandleLimit;
 
            // Position handle so its center sits at _center + delta
            _handle.Position = _center + delta - _handle.Size / 2f;
            _input = delta / HandleLimit;
        }
    }
 
    public Vector2 GetDirection() => _input;
}