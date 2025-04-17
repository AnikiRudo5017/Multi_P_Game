using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Instance;

    private List<string> chatMessages = new List<string>();
    public UIChat chatUI;

    private void Awake()
    {
        Instance = this;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcReceiveChatMessage(int playerID, string message)
    {
        string formattedMessage = $"{playerID}: {message}";
        chatMessages.Add(formattedMessage);

        if (chatUI != null)
            chatUI.chatContent.text += formattedMessage + "\n";
    }

    public void SendChatMessage(string message)
    {
        int playerID = Runner.LocalPlayer.PlayerId;
        RpcReceiveChatMessage(playerID, message);
    }
}
