using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;
using System.IO;

public class PlaceOnIndicator : MonoBehaviour
{
    [SerializeField]
    GameObject placementIndicator;
    private GameObject spawnedObject;

    private List<string> prefabUrls = new List<string> { "https://www.dropbox.com/scl/fi/gyd0cllbuba4jrlxsvtsr/knight.glb?rlkey=2jfaqyaf6hschwcwks9y84fpn&dl=0" };
    //[SerializeField]
    private List<GameObject> availablePrefabs;
    private int currentPrefabIndex = 0;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        placementIndicator.SetActive(false);

        StartCoroutine(LoadPrefabs());
        
    }

    void Update()
    {
        HandlePlacementIndicator();

        // Check for touch or click to place the object
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("MOTHERFUCKERRR");
            PlaceObject();
        }
    }

    void HandlePlacementIndicator()
    {
        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);

            if (!placementIndicator.activeInHierarchy)
                placementIndicator.SetActive(true);
        }
    }

    void PlaceObject()
    {
        if (!placementIndicator.activeInHierarchy)
            return;

        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }

        // Instantiate the currently selected prefab
        spawnedObject = Instantiate(availablePrefabs[currentPrefabIndex], placementIndicator.transform.position, placementIndicator.transform.rotation);
    }

    void SelectPrefab(int index)
    {
        // Change the index to select the next prefab
        currentPrefabIndex = index;

        // You can add visual feedback or UI update for the selected prefab if needed
        Debug.Log("Selected Prefab: " + availablePrefabs[currentPrefabIndex].name);
    }

    IEnumerator LoadPrefabs()
    {
        foreach (string prefabUrl in prefabUrls)
        {
            UnityWebRequest www = UnityWebRequest.Get(prefabUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Extract the name of the prefab file without the file extension
                string prefabFileName = Path.GetFileNameWithoutExtension(prefabUrl);

                // Create a prefab from the downloaded data
                byte[] prefabData = www.downloadHandler.data;
                SaveModelToFile(prefabData);
                Debug.LogError(prefabFileName);
                GameObject prefab = InstantiateFromBytes(prefabData, prefabFileName);
                if (prefab != null)
                {
                    availablePrefabs.Add(prefab);
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab from data: " + prefabUrl);
                }
            }
            else
            {
                Debug.LogError("Failed to download prefab: " + www.error);
            }
        }
    }

    GameObject InstantiateFromBytes(byte[] data, string prefabFileName)
    {
        // Create a prefab from the byte array
        GameObject prefab = new GameObject();
        if (data != null)
        {
            prefab = Instantiate(Resources.Load<GameObject>(prefabFileName));
            // Load or assign data to the prefab (e.g., modify the prefab's components)
        }
        Resources.Load<GameObject>("");
        return prefab;
    }

    void SaveModelToFile(byte[] data)
    {
        // Save the downloaded data to a file
        File.WriteAllBytes("Assets/Resources/DownloadedModel.fbx", data);
        Debug.Log("Model saved to: " + "Assets/Resources/DownloadedModel.fbx");
    }
}
