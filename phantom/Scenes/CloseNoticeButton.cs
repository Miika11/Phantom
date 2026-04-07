using Godot;

public partial class CloseNoticeButton : Button
{
    public override void _Pressed()
    {
        GetParent<Control>().Visible = false;
    }
}