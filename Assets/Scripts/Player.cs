using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]
public class Player : ScriptableObject
{
    [Header("Player Information")]
    public string playerName;
    public int score;
    public string playerId; // Unique ID (có thể là Fusion NetworkId)

    // Reset dữ liệu người chơi khi bắt đầu game mới
    public void ResetData()
    {
        score = 0;
    }

    // Phương thức tăng điểm
    public void AddScore(int points)
    {
        score += points;
    }
}