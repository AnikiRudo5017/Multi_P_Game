using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : NetworkBehaviour
{
    [Networked]
    public int Score { get; set; }
    public TextMeshProUGUI scoreText;

    public override void Spawned()
    {
        Score = 0;
        UpdateScoreUI();
    }
    public void AddScore(int points)
    {
        if (HasStateAuthority)
        {
            Score += points;
            UpdateScoreUI();
        }
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score;
        }
    }
    public override void Render()
    {
        UpdateScoreUI();
    }
}