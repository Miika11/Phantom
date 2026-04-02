using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    public GameManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            QueueFree();
            return;
        }
    }
    #endregion
    	#region Mentor Questions

	// Stores answers: 0 = A, 1 = B, 2 = C
	public List<int> PlayerAnswers = new List<int>();

	public void SaveAnswer(int answerIndex)
	{
		PlayerAnswers.Add(answerIndex);
		GD.Print($"Vastaus tallennettu: {answerIndex}");
	}

	public string GetResult()
	{
		int a = 0, b = 0, c = 0;

		foreach (var answer in PlayerAnswers)
		{
			if (answer == 0) a++;
			else if (answer == 1) b++;
			else if (answer == 2) c++;
		}

		if (a >= b && a >= c)
			return "ETSIJÄ";
		else if (b >= a && b >= c)
			return "ETENIJÄ";
		else
			return "EDISTÄJÄ";
	}

	// Answer reset when reloading game
	public void ResetAnswers()
	{
		PlayerAnswers.Clear();
	}

	#endregion

    #region Game Data

    // Existing Score
    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            _score = Mathf.Clamp(value, 0, 9);
            GD.Print($"Avaimia on nyt: {Score}");
        }
    }

    
    public string SpawnPoint { get; set; } = "StartSpawn";

    
    public int MaxHealth { get; set; } = 3;
    public int CurrentHealth { get; set; } = 3;

    public int Keys { get; set; } = 0;
    public int KeysOnRoomEnter { get; set; } = 0;

    #endregion

    #region Score Methods
    public bool AddScore(int amount)
    {
        if (amount < 0) return false;
        Score += amount;
        return true;
    }

    public bool SubtractScore(int amount)
    {
        if (amount < 0) return false;
        Score -= amount;
        return true;
    }
    #endregion
}