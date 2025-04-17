using UnityEngine;
using Fusion;
using System.Linq;
using Unity.Cinemachine;

public class PlayerDeathManager : NetworkBehaviour
{
    public static PlayerDeathManager Instance;

    public CinemachineCamera spectateCamera;

    private GameObject[] allPlayers;
    private GameObject spectateTarget;
    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartSpectating()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player")
            .Where(p => p.activeSelf)
            .ToArray();

        if (allPlayers.Length > 0)
        {
            currentIndex = 0;
            spectateTarget = allPlayers[currentIndex];

            spectateCamera.Priority = 15;
            spectateCamera.Follow = spectateTarget.transform;
            spectateCamera.LookAt = spectateTarget.transform;
        }
        else
        {
            Debug.Log("Không còn người chơi nào để spectate.");
        }
    }

    private void Update()
    {
        // Phím T để chuyển spectate target
        if (Input.GetKeyDown(KeyCode.T) && spectateCamera.Priority == 15)
        {
            SwitchSpectateTarget();
        }
    }

    private void SwitchSpectateTarget()
    {
        if (allPlayers == null || allPlayers.Length == 0) return;

        currentIndex = (currentIndex + 1) % allPlayers.Length;
        spectateTarget = allPlayers[currentIndex];

        spectateCamera.Follow = spectateTarget.transform;
        spectateCamera.LookAt = spectateTarget.transform;
    }
}
