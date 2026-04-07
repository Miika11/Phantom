using Godot;

public partial class UINoticeBoard : CanvasLayer
{
    private TextureRect noticeImage;
    private Button closeButton;

    public override void _Ready()
    {
        noticeImage = GetNode<TextureRect>("Panel/NoticeImage");
        closeButton = GetNode<Button>("Panel/CloseButton");

        closeButton.Pressed += OnClosePressed;

        Visible = false;
    }

    public void ShowNotice(Texture2D texture)
    {
        noticeImage.Texture = texture;
        Visible = true;
    }

    private void OnClosePressed()
    {
        Visible = false;
    }
}