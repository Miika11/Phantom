using Godot;
using System.Collections.Generic;

public partial class HeartsUI : HBoxContainer
{
    [Export] public Texture2D FullHeart;
    [Export] public Texture2D EmptyHeart;

    private List<TextureRect> _hearts = new List<TextureRect>(); // list to store all health ui elements

    public override void _Ready()
    {
        
        foreach (Node child in GetChildren())
        {
            if (child is TextureRect heart)
                _hearts.Add(heart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < _hearts.Count; i++) // Go through all the hearts
        {
            if (i < currentHealth)
                _hearts[i].Texture = FullHeart; //if index less than current health show full heart else empty heart
            else
                _hearts[i].Texture = EmptyHeart;
        }
    }
}