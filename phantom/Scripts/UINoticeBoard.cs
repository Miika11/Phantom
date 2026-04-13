using Godot;

public partial class UINoticeBoard : CanvasLayer
{
    private TextureRect noticeImage;
    [Export] public Texture2D TextureEN;
    [Export] public Texture2D TextureFI;
    private TextureButton closeButton;

    public override void _Ready()
    {
        noticeImage = GetNode<TextureRect>("NoticeImage");
        closeButton = GetNode<TextureButton>("CloseButton");
        closeButton.Pressed += OnClosePressed;
        Visible = false;
    }

    public void ShowNotice()
    {
        bool isFinnish = GlobalSettings.Instance.GetLocale() == "fi";
        noticeImage.Texture = isFinnish ? TextureFI : TextureEN;
        Visible = true;
    }

    private void OnClosePressed()
    {
        Visible = false;
    }
}