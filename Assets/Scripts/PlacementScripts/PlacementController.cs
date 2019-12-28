using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Selects the preview object that would be placed in the game scene. Also changes the preview object layer to avoid colliding with raycast before placing it.
/// </summary>
public class PlacementController : MonoBehaviour
{
    [SerializeField]

    /// <summary>
    /// Preview objects to instantiate.
    /// </summary>
    [Tooltip("Preview objects to instantiate.")]     
    private GameObject[] placeableObjectPrefabs;

    /// <summary>
    /// Selected Preview object.
    /// </summary>
    private GameObject currentPlacebleObject;

    [HideInInspector]
    public bool isPlaced;
    private int currentObjectIndex  = -1;
    private float mouseWheelRotation;


    // Update is called once per frame
    void Update()
    {
        HandleNewObjectHotkey ();

        if (currentPlacebleObject != null)
        {
            MoveCurrentPlaceableObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }        
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // Moves the preview object to the default layer 
           MoveToLayer(currentPlacebleObject.transform, 0);
           currentPlacebleObject = null;
           isPlaced = true;
        }
        
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlacebleObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentPlacebleObject.transform.position = hitInfo.point;
            currentPlacebleObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void HandleNewObjectHotkey()
    {
        for (int i = 0; i < placeableObjectPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                if (pressedKeyOfCurrentObject(i))
                {
                    Destroy(currentPlacebleObject);
                    currentObjectIndex = -1;
                }   else
                    {
                        if (currentPlacebleObject != null)
                        {
                            Destroy(currentPlacebleObject);
                        }     

                        currentPlacebleObject = Instantiate (placeableObjectPrefabs [i]);
                        
                        // Moves the preview object to the ignore raycast layer
                        MoveToLayer(currentPlacebleObject.transform, 2); 
                        currentObjectIndex = i;            
                    }

                isPlaced = false;
                break;    
            }            
        }

    }

    // Removes the current preview object from display if its key is pressed again.
    private bool pressedKeyOfCurrentObject(int i)
    {
        return currentPlacebleObject != null && currentObjectIndex == i;
    }

    // Changes the layer of the preview object and all its children.
    void MoveToLayer (Transform root, int layer){
        root.gameObject.layer = layer;
        foreach (Transform child in root)
        {
            MoveToLayer (child, layer);
        }
    }
}
