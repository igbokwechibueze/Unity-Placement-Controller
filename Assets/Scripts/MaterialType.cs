using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Defines the type of materials objects can be made from. This aids guns to instantiate hit effects and hit sounds, unique to a particular material.
/// </summary>
public class MaterialType : MonoBehaviour {

    public MaterialTypeEnum TypeOfMaterial = MaterialTypeEnum.Plaster;

    [System.Serializable]
	public enum MaterialTypeEnum
	{
        Plaster,
	    Metall,
        Folliage,
        Rock,
        Wood,
        Brick,
        Concrete,
        Dirt,
        Glass,
        Water,
        Flesh,
	}
}
