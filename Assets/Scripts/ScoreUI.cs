using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
	[SerializeField] private ScoreTracker scoreTracker;
	[SerializeField] private TextMeshProUGUI textMesh;

	private void Awake()
	{
		scoreTracker.OnScoreUpdated += UpdateValue;
	}

	private void UpdateValue(ScoreTracker tracker, int oldVal, int newVal)
	{
		if (tracker != scoreTracker) return;

		textMesh.text = newVal.ToString();
	}
}
