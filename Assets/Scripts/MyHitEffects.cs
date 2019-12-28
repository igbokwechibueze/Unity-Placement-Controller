using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHitEffects : MonoBehaviour
{
    public ImpactInfo[] ImpactElemets = new ImpactInfo[0];
    MaterialType materialType;

    [System.Serializable]
    public class ImpactInfo
    {
        public MaterialType.MaterialTypeEnum MaterialType;
        public GameObject ImpactEffect;
    }   

    public GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var materialType = impactedGameObject.GetComponent<MaterialType>();
        if (materialType==null)
            return null;
        foreach (var impactInfo in ImpactElemets)
        {
            if (impactInfo.MaterialType == materialType.TypeOfMaterial)
                return impactInfo.ImpactEffect;
        }
        return null;
    }
}
