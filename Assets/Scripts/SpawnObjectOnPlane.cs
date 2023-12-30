using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ModelInteract : MonoBehaviour
{
    
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedPrefabList = new List<GameObject>();
    private int state = 0;

    [SerializeField]
    private int maxPrefabSpawnCount =  0;
    private int placedPrefabCount;

    [SerializeField]
    private GameObject placeablePrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private Touch touch;
    private Vector2 touchStartPos;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchPosition = touch.position;
                return true;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                touchPosition = touch.position;
                return true;
            }
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if(raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;

            if(placedPrefabCount < maxPrefabSpawnCount)
            {
                SpawnPrefab(hitPose);
            }

            if (spawnedObject != null)
            {
                RotateAndMoveObject(touchPosition);
            }
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

    private void RemoveLastSpawnedPrefab()
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
        float deltaX = touchPosition.x - touchStartPos.x;
        float rotationAngle = deltaX * 0.5f;

        // Rotate the spawned object
        spawnedObject.transform.Rotate(Vector3.up, -rotationAngle);

        // Update the touch starting position for the next frame
        touchStartPos = touchPosition;

        // Move the spawned object forward and backward based on touch movement
        float deltaY = touchPosition.y - touchStartPos.y;
        float moveDistance = deltaY * 0.01f;

        spawnedObject.transform.Translate(Vector3.forward * moveDistance);
    }


}