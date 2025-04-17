using UnityEngine;
using Fusion;

public class SelfDespawn : NetworkBehaviour
{
    private bool isDespawned = false;

    private void Update()
    {
        if (!HasInputAuthority || isDespawned) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            isDespawned = true;

            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Object);
            }

            // Gọi chế độ spectate cho local client
            PlayerDeathManager.Instance.StartSpectating();
        }
    }
}
