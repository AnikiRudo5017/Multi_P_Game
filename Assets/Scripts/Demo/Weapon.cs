using System;
using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcApplyDamageToPlayer(PlayerRef targetPlayer, int damage)
    {
        Runner.TryGetPlayerObject(targetPlayer, out var plObject);
        if (plObject == null)
        {
            return;
        }
        plObject.GetComponent<PlayerProperties>().TakeDamage(damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var targetPlayer = other.gameObject.GetComponent<NetworkObject>().InputAuthority;
            RpcApplyDamageToPlayer(targetPlayer, 10);
        }
    }
}
