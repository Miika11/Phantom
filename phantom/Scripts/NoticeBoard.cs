using Godot;

public partial class NoticeBoard : Node2D
{
    [Export] public Texture2D NoticeTexture;
    [Export] public UINoticeBoard NoticeUI;

    public override void _Ready()
    {
        var area = GetNode<Area2D>("Area2D");
        area.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("player"))
        {
            NoticeUI.ShowNotice(NoticeTexture);
        }
    }
}