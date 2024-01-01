using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChosenPrefab))]
public class ChangeColor : MonoBehaviour
{
    private GameObject model;
    public Material[] material;
    public int size;
    Renderer rend;

    public void NextColor()
    {
        model = GetComponent<ChosenPrefab>().model();
        size = 0;
        rend = model.GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[size];

        if (size < 2)
        {
            size ++;
        }
        else
        {
            size = 0;
        }
    }
}
