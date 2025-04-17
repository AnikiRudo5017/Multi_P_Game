using Fusion;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject notificationText;
    [Networked] private int PlayerCount { get; set; }
    private bool isRoomMaster = false;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            PlayerCount++;
        }
        if (PlayerCount == 1 && Object.HasStateAuthority)
        {
            isRoomMaster = true;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (isRoomMaster)
        {
            startButton.gameObject.SetActive(true);
            notificationText.gameObject.SetActive(false);
        }
        else
        {
            startButton.gameObject.SetActive(false);
            notificationText.gameObject.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        if (isRoomMaster)
        {
            RPC_StartGame();
        }
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_StartGame()
    {
        if (Runner.IsSceneAuthority)
        {
            Runner.LoadScene(SceneRef.FromIndex(1), LoadSceneMode.Additive);
        }
    }
}