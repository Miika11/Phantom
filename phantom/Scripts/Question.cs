using Godot;

public class Question
{
    public string Text;
    public string[] Answers;

    public Question(string text, string[] answers)
    {
        Text = text;
        Answers = answers;
    }
}