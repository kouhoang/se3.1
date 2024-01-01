using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBar : MonoBehaviour
{
    [SerializeField]
    GameObject bg;
    bool isRotating = false;
    bool isHid = false;
    public void pressedRotate()
    {
        if (isRotating)
        {
            isRotating = false;
            bg.SetActive(false);
        }
        else
        {
            isRotating = true;
            bg.SetActive(true);
        }
    }

    public void ShowHide()
    {
        isHid = !isHid;
        if (isHid)
        {
            transform.position = new Vector3(-60, 855, 0);
        }
        else
        {
            transform.position = new Vector3(60, 855, 0);
        }
    }
}
