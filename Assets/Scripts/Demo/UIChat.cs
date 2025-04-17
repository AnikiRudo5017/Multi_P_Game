using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI chatContent;

    private void Start()
    {
        sendButton.onClick.AddListener(SendMessage);
    }

    private void SendMessage()
    {
        string message = inputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            ChatManager.Instance.SendChatMessage(message);
            inputField.text = "";//delete the content after send
        }
    }
}
