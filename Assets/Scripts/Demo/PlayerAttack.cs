using System;
using Fusion;
using UnityEngine;

public class PlayerAttack : NetworkObject
{
    public GameObject weapon;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.GetComponent<BoxCollider>().enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
