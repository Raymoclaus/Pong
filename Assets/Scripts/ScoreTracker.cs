using System;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
	public int Score { get; private set; }

	public void AddScore(int amount)
	{
		int oldScore = Score;
		Score += amount;
		OnScoreUpdated?.Invoke(this, oldScore, Score);
	}

	public void IncrementScore() => AddScore(1);

	public event Action<ScoreTracker, int, int> OnScoreUpdated;
}
