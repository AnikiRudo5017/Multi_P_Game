using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "Configuration", menuName = "Scriptable Objects/Configuration")]
public class Configuration : ScriptableObject
{
    public bool gameStatus = false;
    public int quantityPlayer = 4;
}
