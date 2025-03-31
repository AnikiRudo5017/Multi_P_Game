using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpwaner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 5, Random.Range(-5f, 5f));
            //Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            Runner.Spawn(PlayerPrefab, randomPosition, Quaternion.identity,
                Runner.LocalPlayer, (runner, obj) =>
                {
                    var _player = obj.GetComponent<PlayerSetup>();
                    _player.SetupCamera();
                }
            );
        }
    }
}
