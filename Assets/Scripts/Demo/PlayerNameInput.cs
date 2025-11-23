using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    public GameObject namePanel;
    public TMP_InputField nameInputField;
    public Button confirmButton;
    public TextMeshProUGUI name;

    public static string LocalPlayerName { get; private set; }

    void Start()
    {
        namePanel.SetActive(true);
        confirmButton.onClick.AddListener(ConfirmName);
    }

    void ConfirmName()
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            LocalPlayerName = playerName; // Lưu tên vào biến static
            if (name != null)
            {
                name.text = playerName; // Gán tên vào TextMeshProUGUI
            }
            namePanel.SetActive(false); // Tắt panel nhập tên
        }
    }
}