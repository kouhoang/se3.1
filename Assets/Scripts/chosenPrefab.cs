using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChosenPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject placeablePrefab;
    // Update is called once per frame
    public GameObject model()
    {
        return placeablePrefab;
    }

    public void SetPrefabType(GameObject prefabChosen)
    {
        placeablePrefab = prefabChosen;
    }

}
