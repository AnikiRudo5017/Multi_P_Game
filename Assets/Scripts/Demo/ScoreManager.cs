using Fusion;
using TMPro;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public TextMeshProUGUI scoreText;
    
    [Networked, OnChangedRender(nameof(OnScoreChanged))]
    public int Score { get; set; }

    public override void Spawned()
    {
        Score = 0;
        UpdateScoreDisplay();
    }

    private void OnScoreChanged()
    {
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Score}";
        }
    }
    
    public void AddScore(int points)
    {
        if (Object.HasStateAuthority)
        {
            Score += points;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Score"))
        {
            RPC_AddPoint();
            if (Object.HasStateAuthority)
            {
                Runner.Despawn(other.GetComponent<NetworkObject>());
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_AddPoint()
    {
        AddScore(1);
    }
}