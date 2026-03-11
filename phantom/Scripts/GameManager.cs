using Godot;
using System;

public partial class GameManager : Node
{
	#region Singleton
	public static GameManager Instance
	{
		get;
		private set;
	}

	public GameManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			QueueFree();
			return;
		}
	}
	#endregion

	#region Game Data
	private int _score = 0;

	public int Score
	{
		get { return _score; }
		set
		{
			_score = Mathf.Clamp(value, 0, 9);
			GD.Print($"Avaimia on nyt: {Score}");
		}
	}
	#endregion

	public bool AddScore(int amount)
	{
		if (amount < 0)
		{
			return false;
		}
		
		Score += amount;
		return true;
	}

	public bool SubtractScore(int amount)
	{
		if (amount < 0)
		{
			return false;
		}
		
		Score -= amount;
		return true;
	}
}

