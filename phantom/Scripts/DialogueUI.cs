using Godot;
using System.Collections.Generic;

public partial class DialogueUI : CanvasLayer
{
    private Label _questionLabel;
    private Button _a, _b, _c;
    private Label _resultLabel;
    private TextureRect _backgroundImage;

    private Button _closeButton; 

    private List<Question> _questions;
    private int _currentQuestion = 0;

    public override void _Ready()
    {
        _backgroundImage = GetNode<TextureRect>("Panel/BackgroundImage");
        _questionLabel = GetNode<Label>("Panel/QuestionLabel");
        _a = GetNode<Button>("Panel/AnswerA");
        _b = GetNode<Button>("Panel/AnswerB");
        _c = GetNode<Button>("Panel/AnswerC");
        _resultLabel = GetNode<Label>("Panel/ResultLabel");
        

        _closeButton = GetNode<Button>("Panel/CloseButton"); 

        _a.Pressed += () => Answer(0);
        _b.Pressed += () => Answer(1);
        _c.Pressed += () => Answer(2);
        

        _closeButton.Pressed += OnClosePressed;

        Hide();
        _resultLabel.Hide();
        _closeButton.Hide();
    }

    public void SetBackground(Texture2D texture)
    {
        _backgroundImage.Texture = texture;
    }

    public void StartDialogue(List<Question> questions)
    {
        _questions = questions;
        _currentQuestion = 0;

        _resultLabel.Hide();
        _closeButton.Hide(); // Hide close button during questions
        _a.Show(); _b.Show(); _c.Show();

        Show();
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        var q = _questions[_currentQuestion];
        _questionLabel.Text = q.Text;
        _a.Text = "A: " + q.Answers[0];
        _b.Text = "B: " + q.Answers[1];
        _c.Text = "C: " + q.Answers[2];
    }

    private void Answer(int index)
    {
        GameManager.Instance.SaveAnswer(index);
        _currentQuestion++;

        if (_currentQuestion >= _questions.Count)
        {
            Hide();
        }
        else
        {
            ShowQuestion();
        }
    }

    public void ShowFinalResult(string result)
    {
        Show();

        _questionLabel.Text = Tr("RESULT_LABEL");
        _a.Hide(); _b.Hide(); _c.Hide();

        _resultLabel.Show();
        _closeButton.Show();

        if (result == "ETSIJÄ")
            _resultLabel.Text = Tr("RESULT_SEEKER_TITLE") + "\n\n" + Tr("RESULT_SEEKER_TEXT");
        else if (result == "ETENIJÄ")
            _resultLabel.Text = Tr("RESULT_ADVANCER_TITLE") + "\n\n" + Tr("RESULT_ADVANCER_TEXT");
        else
            _resultLabel.Text = Tr("RESULT_IDEA_TITLE") + "\n\n" + Tr("RESULT_IDEA_TEXT");

        _resultLabel.AddThemeColorOverride("font_color", Colors.Black);
    }


    private void OnClosePressed()
    {
        Hide();

    }
}