using UnityEngine;

public class ChatOnOff : MonoBehaviour
{
    [SerializeField] private GameObject chatCanvas;
    private bool isChatActive = false;

    void Start()
    {
        if (chatCanvas != null)
        {
            chatCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ToggleChat();
        }
    }

    private void ToggleChat()
    {
        // Đổi trạng thái hiển thị
        isChatActive = !isChatActive;
        
        // Cập nhật trạng thái Canvas
        if (chatCanvas != null)
        {
            chatCanvas.SetActive(isChatActive);
        }

        // (Tùy chọn) Nếu bạn muốn focus vào InputField khi mở chat
        if (isChatActive && chatCanvas.GetComponentInChildren<TMPro.TMP_InputField>() != null)
        {
            TMPro.TMP_InputField inputField = chatCanvas.GetComponentInChildren<TMPro.TMP_InputField>();
            inputField.ActivateInputField();
        }
    }
}
