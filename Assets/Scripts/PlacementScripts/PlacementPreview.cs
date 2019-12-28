using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates the desired game object on the desired surface by checking its tag.
/// </summary>
public class PlacementPreview : MonoBehaviour
{
    /// <summary>
    /// Prefab to instantiate.
    /// </summary>
    [Tooltip("Prefab to instantiate.")]   
    public GameObject placementPrefab;
    
    private MeshRenderer[] meshRenderers;

    /// <summary>
    /// Material to indicate that the prefab can be placed in the desired position.
    /// </summary>
    [Tooltip("Material to indicate that the prefab can be placed in the desired position.")]
    public Material goodMaterial;

    /// <summary>
    /// Material to indicate that the prefab cannot be placed in the desired position.
    /// </summary>
    [Tooltip("Material to indicate that the prefab cannot be placed in the desired position.")]    
    public Material badMaterial;

    /// <summary>
    /// Indicates if we can place a prefab in a desired area, by changing the material of the placementPrefab.
    /// </summary>
    private bool canBuild;

    /// <summary>
    /// A list of tags you cannot instantiate a prefab on.
    /// </summary>
    public List<string> tagsNotToSnapTo = new List<string>();

    /// <summary>
    /// Refrence to the script controlling placement.
    /// </summary>
    PlacementController placementController;

    /// <summary>
    /// How long it will take to instantiate the placementprefab.
    /// </summary>
    public float timeToInstantiate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        ChangeColor();
        placementController = FindObjectOfType<PlacementController>();
    }

    private void Update() 
    {

        // When the placementController script has placed the preview prefab. Begin routine to place the placement prefab.
        if (placementController.isPlaced == true)
        {
            if (canBuild == true)
            {
                StartCoroutine(Place());
                return;
            }else
                {
                    // Destroy this preview prefab from the scene, leaving behind the placement prefab.
                    Destroy(gameObject);
                }
        }                
    }

    IEnumerator Place ()
    {
        // waits a set amount of time before instantiating the desired prefab.
        yield return new WaitForSeconds (timeToInstantiate);
        Instantiate (placementPrefab, transform.position, transform.rotation);
        Destroy(gameObject);        
    }

    //Changes the color of the placement preview object to indicate if it can be placed in the desired location or not.
    void ChangeColor ()
    {
        if (canBuild == true)
        {
            foreach (Renderer color in meshRenderers)
            {
                color.material = goodMaterial;
            }
        }else
        {
            foreach (Renderer color in meshRenderers)
            {
                color.material = badMaterial;
            }            
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        // checks the tag of the surface to know if objects can be placed on it or not.
        if (tagsNotToSnapTo != null)
        {
            for (int i = 0; i < tagsNotToSnapTo.Count; i++)
            {
                string currentTag = tagsNotToSnapTo[i];
                if (other.tag != currentTag)
                {
                    canBuild = true;
                    ChangeColor();
                }else
                    {
                        canBuild = false;
                        ChangeColor();
                    }
            }        
        }else
        {
            canBuild = true;
            ChangeColor();
        }

    }
}
 