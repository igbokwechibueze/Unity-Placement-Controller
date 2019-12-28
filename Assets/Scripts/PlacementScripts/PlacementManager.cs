using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// controls which weapon groups should be active.
/// Weapon groups are Hand Held Weapons, Turret placements, Explosive placements and Drone/Air support.
/// </summary>
public class PlacementManager : MonoBehaviour
{
    /// <summary>
    /// The script that controlls your hand held weapons, like machine gun, pistols, etc.
    /// </summary>
    [Tooltip("The script that controlls your hand held weapons, like machine gun, pistols, etc.")]
    public WeaponInventory weaponGroup1;

    /// <summary>
    /// The gameobject controlling turret placement.
    /// </summary>
    [Tooltip("The gameobject controlling turret placement.")]    
    public GameObject weaponGroup2;

    /// <summary>
    /// The gameobject controlling explosive placement.
    /// </summary>
    [Tooltip("The gameobject controlling explosive placement.")]        
    public GameObject weaponGroup3;

    /// <summary>
    /// The gameobject controlling air support placement.
    /// </summary>
    [Tooltip("The gameobject controlling air support placement.")]    
    public GameObject weaponGroup4;

    /// <summary>
    /// The script that controlls the animation of the weapon pie menu.
    /// </summary>
    [Tooltip("The script that controlls the animation of the weapon pie menu.")]
    public PieMenuAnim pieMenuAnim;


    private void Start() 
    {
        // sets hand held weapon group as default on game start.
        weaponGroup1.enabled = true; 
    }

// *********** The functions below activates a selected weapon group while deactivating the rest. It aslo deactivates and activates the weapon pie menu **************    
    public void ActivateweaponGroup1 ()
    {
        weaponGroup1.enabled = true;
        weaponGroup2.SetActive(false);
        weaponGroup3.SetActive(false);
        weaponGroup4.SetActive(false);

        pieMenuAnim.AnimatePanel ();

    }

    public void ActivateweaponGroup2 ()
    {
        weaponGroup1.enabled = false;
        weaponGroup2.SetActive(true);
        weaponGroup3.SetActive(false);
        weaponGroup4.SetActive(false);        

        pieMenuAnim.AnimatePanel ();
    }

    public void ActivateweaponGroup3 ()
    {
        weaponGroup1.enabled = false;
        weaponGroup2.SetActive(false);
        weaponGroup3.SetActive(true);
        weaponGroup4.SetActive(false);

        pieMenuAnim.AnimatePanel ();
    }

    public void ActivateweaponGroup4 ()
    {
        weaponGroup1.enabled = false;
        weaponGroup2.SetActive(false);
        weaponGroup3.SetActive(false);
        weaponGroup4.SetActive(true);

        pieMenuAnim.AnimatePanel ();
    }
}
