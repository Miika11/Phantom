using Godot;

public partial class BoostButton : Control
{
    [Export] public Texture2D NormalTexture;
    [Export] public Texture2D PressedTexture;

    [Signal] public delegate void BoostStartedEventHandler();
    [Signal] public delegate void BoostStoppedEventHandler();

    private int _touchIndex = -1;
    private TextureRect _textureRect;

    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TextureRect");
        _textureRect.Texture = NormalTexture;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touch)
        {
            if (touch.Pressed && _touchIndex == -1)
            {
                _touchIndex = touch.Index;
                _textureRect.Texture = PressedTexture;
                EmitSignal(SignalName.BoostStarted);
                AcceptEvent();
            }
            else if (!touch.Pressed && touch.Index == _touchIndex)
            {
                _touchIndex = -1;
                _textureRect.Texture = NormalTexture;
                EmitSignal(SignalName.BoostStopped);
                AcceptEvent();
            }
        }
    }
}