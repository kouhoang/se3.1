using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ModelInteract : MonoBehaviour
{
    private ARPlaneManager arplaneManager;
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedPrefabList = new List<GameObject>();

    [SerializeField]
    private int maxPrefabSpawnCount =  0;

    private int placedPrefabCount;

    [SerializeField]
    private GameObject placeablePrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    // Touch variables initiate
    private Touch touch;
    private Vector2 touchStartPos;
    //private Vector2 firstTouch;
    private Vector2 secondTouch;
    private float distanceCurrent;
    private float distancePrevious;
    private bool firstPinch;

    [SerializeField]
    private float rotationSpeed = 1.0f;

    [SerializeField]
    private float moveSpeed = 0.01f;

    private void Awake()
    {
        arplaneManager = GetComponent<ARPlaneManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (spawnedObject == null)
        {
            raycastManager.enabled = true;
            arplaneManager.enabled = true;
        }
        if (Input.touchCount > 0)
        {
            if (raycastManager.Raycast(Input.GetTouch(0).position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    touchStartPos = Input.GetTouch(0).position;
                    if (placedPrefabCount < maxPrefabSpawnCount && spawnedObject == null)
                    {
                        var hitPose = s_Hits[0].pose;
                        SpawnPrefab(hitPose);
                        raycastManager.enabled = false;
                        arplaneManager.enabled = false;
                    }
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (spawnedObject != null)
                    {
                        RotateAndMoveObject(Input.GetTouch(0).position);
                    }
                }
            }
        }
        
        if (Input.touchCount > 1)
        {
            PinchToScale();
        }
        else
        {
            firstPinch = true;
        }
    }

    public void SetPrefabType(GameObject prefabChosen)
    {
        placeablePrefab = prefabChosen;
    }

    private void SpawnPrefab(Pose hitPose)
    {
        spawnedObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
        placedPrefabList.Add(spawnedObject);
        placedPrefabCount++;
    }

    public void RemoveLastSpawnedPrefab()
    {
        if (placedPrefabList.Count > 0)
        {
            GameObject lastSpawnedPrefab = placedPrefabList[placedPrefabList.Count - 1];
            Destroy(lastSpawnedPrefab);
            placedPrefabList.RemoveAt(placedPrefabList.Count - 1);
            placedPrefabCount--;
        }
    }

    private void RotateAndMoveObject(Vector2 touchPosition)
    {
        // Calculate the rotation angle based on touch movement
        //float deltaX = touchPosition.x - touchStartPos.x;
        //float rotationAngle = deltaX * 0.5f;

        // Rotate the spawned object
        //spawnedObject.transform.Rotate(Vector3.up, -rotationAngle);

        // Update the touch starting position for the next frame
        //touchStartPos = touchPosition;

        // Move the spawned object forward and backward based on touch movement
        float deltaY = touchPosition.y - touchStartPos.y;
        float moveDistance = deltaY * 0.01f;

        spawnedObject.transform.Translate(Vector3.forward * moveDistance);
    }

    private void PinchToScale()
    {
        //firstTouch = Input.GetTouch(0).position;
        secondTouch = Input.GetTouch(1).position;
        distanceCurrent = secondTouch.magnitude - touchStartPos.magnitude;

        if(firstPinch)
        {
            distancePrevious = distanceCurrent;
            firstPinch = false;
        }

        if(distanceCurrent != distancePrevious)
        {
            Vector3 scale_value = spawnedObject.transform.localScale * (distanceCurrent / distancePrevious);
            spawnedObject.transform.localScale = scale_value;
            distancePrevious = distanceCurrent;
        }
    }
}