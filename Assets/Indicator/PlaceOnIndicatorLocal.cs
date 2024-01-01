using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class PlaceOnIndicatorLocal : MonoBehaviour
{
    [SerializeField]
    GameObject placementIndicator;
    private GameObject spawnedObject;

    [SerializeField]
    [Tooltip("Input list of placeable prefabs.")]
    private List<GameObject> availablePrefabs;
    private int currentPrefabIndex = 0;

    [SerializeField]
    [Tooltip("Adjust prefab rotation sensitivity.")]
    float rotationSensitivity = 0.3f;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    // Rotation variables
    private bool isRotating = false;
    private Vector2 rotationStartPos;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        placementIndicator.SetActive(false);
    }

    void Update()
    {
        HandlePlacementIndicator();
        if (Input.touchCount > 0)
        {
            HandleFreeTouch();
        }
    }

    public void resetPlane()
    {
        if (spawnedObject != null)
            Destroy(spawnedObject);
        ARSession session = FindObjectOfType<ARSession>();
        if (session != null)
            session.Reset();
    }
    public void rotateState()
    {
        if (isRotating)
        {
            isRotating = false;
        }
        else
        {
            isRotating = true;
        }
    }

    void HandleFreeTouch()
    {
        // Check for touch or click to place the object
        Touch touch = Input.GetTouch(0);

        if (isRotating)
        {
            if (touch.phase == TouchPhase.Began)
            {
                rotationStartPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                RotateObject(touch.position);
            }
        }
        
    }

    void HandlePlacementIndicator()
    {
        // Check if the indicator is within plane
        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);

            // Activate the disabled Indicator
            if (!placementIndicator.activeInHierarchy && spawnedObject == null)
                placementIndicator.SetActive(true);
        }
    }
    /*
     * if (spawnedObject == null)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (!isRotating && !isChoosing)
                {
                    PlaceObject();
                }
            }
        }
        else
     */
    public void PlaceObject()
    {
        if (isRotating)
            return;
        // If Object spawned or Indicator is out of plane site then ignore placement
        if (!placementIndicator.activeInHierarchy)
            return;
        if (spawnedObject != null)
            return;
        // Instantiate the currently selected prefab
        spawnedObject = Instantiate(availablePrefabs[currentPrefabIndex], placementIndicator.transform.position, placementIndicator.transform.rotation);
        placementIndicator.SetActive(false);
    }

    public void DestroyObject()
    {
        // Destroy spawned prefab
        if (spawnedObject != null)
            Destroy(spawnedObject);
    }
        

    public void SelectPrefab(int index)
    {
        // Change the index to select the next prefab
        if (index < availablePrefabs.Count)
            currentPrefabIndex = index;

        // You can add visual feedback or UI update for the selected prefab if needed
        Debug.Log("Selected Prefab: " + availablePrefabs[currentPrefabIndex].name);
    }

    void StartRotation(Vector2 touchPosition)
    {
        // Check if the touch is over the spawned object
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == spawnedObject.transform)
        {
            isRotating = true;
            rotationStartPos = touchPosition;
        }
    }

    void RotateObject(Vector2 touchPosition)
    {
        // Calculate the rotation angle based on touch movement
        Vector2 deltaRotation = touchPosition - rotationStartPos;
        float rotationAngle = deltaRotation.x * rotationSensitivity;

        // Apply rotation to the spawned object
        spawnedObject.transform.Rotate(Vector3.up, -rotationAngle);

        // Update the rotation start position for the next frame
        rotationStartPos = touchPosition;
    }
}
