using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fusion;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private GameObject scoreboardPanel; // Panel chứa danh sách người chơi
    [SerializeField] private GameObject playerEntryPrefab; // Prefab cho mỗi entry (TextMeshProUGUI)
    [SerializeField] private Transform playerEntryContainer; // Container cho các entry

    private NetworkRunner runner;
    private Dictionary<PlayerRef, GameObject> playerEntries = new Dictionary<PlayerRef, GameObject>();

    void Start()
    {
        runner = FindObjectOfType<NetworkRunner>();
        if (runner == null)
        {
            Debug.LogError("NetworkRunner not found!");
        }
    }

    void Update()
    {
        if (runner == null || !runner.IsRunning)
            return;

        // Cập nhật Scoreboard
        UpdateScoreboard();
    }

    private void UpdateScoreboard()
    {
        // Thêm hoặc cập nhật entry cho người chơi
        foreach (var player in runner.ActivePlayers)
        {
            if (!playerEntries.ContainsKey(player))
            {
                // Tạo entry mới
                GameObject entry = Instantiate(playerEntryPrefab, playerEntryContainer);
                entry.SetActive(true);
                playerEntries[player] = entry;
            }

            // Cập nhật thông tin
            NetworkObject playerObject = runner.GetPlayerObject(player);
            if (playerObject != null)
            {
                PlayerNetwork playerNetwork = playerObject.GetComponent<PlayerNetwork>();
                ScoreManager scoreManager = playerObject.GetComponent<ScoreManager>();
                if (playerNetwork != null && scoreManager != null)
                {
                    TextMeshProUGUI text = playerEntries[player].GetComponent<TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.text = $"{playerNetwork.PlayerName}: {scoreManager.Score}";
                    }
                }
            }
        }

        // Xóa/entry của người chơi đã rời
        List<PlayerRef> toRemove = new List<PlayerRef>();
        foreach (var entry in playerEntries)
        {
            if (!runner.ActivePlayers.Contains(entry.Key))
            {
                Destroy(entry.Value);
                toRemove.Add(entry.Key);
            }
        }
        foreach (var player in toRemove)
        {
            playerEntries.Remove(player);
        }
    }
}