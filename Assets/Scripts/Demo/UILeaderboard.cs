using Fusion;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UILeaderboard : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;
    public KeyCode toggleKey = KeyCode.Tab;
    public float updateInterval = 1f;
    
    private bool _isVisible = false;
    private float _lastUpdateTime;
    private NetworkRunner _runner;
    
    private void Start()
    {
        gameObject.SetActive(false);
        _lastUpdateTime = 0f;
        
        // Tìm NetworkRunner trong scene
        _runner = FindObjectOfType<NetworkRunner>();
        if (_runner == null)
        {
            Debug.LogWarning("NetworkRunner không được tìm thấy. Leaderboard có thể không hoạt động.");
        }
    }
    
    private void Update()
    {
        // Hiển thị/ẩn bảng điểm khi nhấn phím được cấu hình
        if (Input.GetKeyDown(toggleKey))
        {
            _isVisible = !_isVisible;
            gameObject.SetActive(_isVisible);
            
            if (_isVisible)
            {
                UpdateLeaderboard();
            }
        }
        
        // Cập nhật bảng điểm theo khoảng thời gian khi đang hiển thị
        if (_isVisible && Time.time - _lastUpdateTime >= updateInterval)
        {
            UpdateLeaderboard();
            _lastUpdateTime = Time.time;
        }
    }
    
    private void UpdateLeaderboard()
    {
        if (_runner == null)
        {
            _runner = FindObjectOfType<NetworkRunner>();
            if (_runner == null) return;
        }

        string leaderboardString = "BẢNG XẾP HẠNG\n\n";
        
        // Thu thập thông tin điểm số của người chơi
        var playerScores = new List<PlayerScoreInfo>();
        
        foreach (var playerObj in FindObjectsOfType<PlayerNetwork>())
        {
            if (playerObj != null)
            {
                var scoreManager = playerObj.GetComponent<ScoreManager>();
                if (scoreManager != null)
                {
                    string playerName = playerObj.PlayerName.ToString();
                    if (string.IsNullOrEmpty(playerName))
                    {
                        playerName = "Player " + playerObj.Object.InputAuthority.PlayerId;
                    }
                    
                    playerScores.Add(new PlayerScoreInfo
                    {
                        Name = playerName,
                        Score = scoreManager.Score,
                        IsLocalPlayer = playerObj.Object.HasInputAuthority
                    });
                }
            }
        }
        
        // Sắp xếp theo điểm số giảm dần
        playerScores = playerScores.OrderByDescending(p => p.Score).ToList();
        
        // Tạo chuỗi hiển thị
        for (int i = 0; i < playerScores.Count; i++)
        {
            string playerString = $"{i+1}. {playerScores[i].Name}: {playerScores[i].Score} điểm";
            
            // Đánh dấu người chơi hiện tại bằng màu vàng
            if (playerScores[i].IsLocalPlayer)
            {
                playerString = $"<color=yellow>{playerString}</color>";
            }
            
            leaderboardString += playerString + "\n";
        }
        
        if (playerScores.Count == 0)
        {
            leaderboardString += "Chưa có người chơi nào.";
        }
        
        // Cập nhật UI
        if (leaderboardText != null)
        {
            leaderboardText.text = leaderboardString;
        }
    }
    
    // Lớp chứa thông tin điểm số của người chơi
    private class PlayerScoreInfo
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsLocalPlayer { get; set; }
    }
}