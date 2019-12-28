using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Lets radars know if the gameobject is a surface based object or an aircraft.
/// It also distinguish enermy objects from allies.
/// </summary>
public class KindOfTarget : MonoBehaviour
{

    [System.Serializable]
    public enum TargetType
    {
        Land,
        Air
    }

    public TargetType targetType = TargetType.Air;

    /// <summary>
    /// Is this gameobject an enemy to the player.
    /// </summary>
    [Tooltip("Is this gameobject an enemy to the player.")]      
    public bool isEnemy;

}
