using Godot;

public partial class Joystick : Control
{
    [Export] public float HandleLimit = 50f;
    private Vector2 _input = Vector2.Zero;
    private Vector2 _startPosition;
    private TextureRect _handle;
    private int _touchIndex = -1; 

    public override void _Ready()
    {
        _handle = GetNode<TextureRect>("Handle");
        _startPosition = _handle.Position;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touch)
        {
            if (touch.Pressed && _touchIndex == -1)
            {
                
                _touchIndex = touch.Index;
                _handle.Position = _startPosition;
            }
            else if (!touch.Pressed && touch.Index == _touchIndex)
            {
                
                _touchIndex = -1;
                _input = Vector2.Zero;
                _handle.Position = _startPosition;
            }
        }
        else if (@event is InputEventScreenDrag drag)
        {
            
            if (drag.Index != _touchIndex) return;

            Vector2 delta = drag.Position - _startPosition;
            if (delta.Length() > HandleLimit)
                delta = delta.Normalized() * HandleLimit;
            _handle.Position = _startPosition + delta;
            _input = delta / HandleLimit;
        }
    }

    public Vector2 GetDirection() => _input;
}