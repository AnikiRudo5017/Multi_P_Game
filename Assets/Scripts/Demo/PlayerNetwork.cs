using Fusion;
using UnityEngine;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    [Networked]
    public NetworkString<_16> PlayerName { get; set; }

    public TMP_Text nameText;

    public override void Spawned()
    {
        // Khi nhân vật được sinh ra, hiển thị tên nếu đã có
        nameText.text = PlayerName.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetName(string newName)
    {
        PlayerName = newName;

        // Cập nhật lại UI thủ công trên máy host/server
        UpdateNameUI();

        // Gửi thông tin tên cho mọi client thông qua Tick Delay một cách gián tiếp
    }

    public override void Render()
    {
        // Render được gọi trên mọi client, mỗi frame.
        // Dùng nó để chắc chắn UI luôn được đồng bộ
        if (nameText != null && nameText.text != PlayerName.ToString())
        {
            nameText.text = PlayerName.ToString();
        }
    }

    private void UpdateNameUI()
    {
        if (nameText != null)
        {
            nameText.text = PlayerName.ToString();
        }
    }
}