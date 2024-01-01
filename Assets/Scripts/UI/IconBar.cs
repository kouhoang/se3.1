using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBar : MonoBehaviour
{
    [SerializeField]
    GameObject rotateBg;
    [SerializeField]
    GameObject listBg;
    bool isRotating = false;
    bool isList = false;
    bool isHid = false;
    public void pressedRotate()
    {
        if (isRotating)
        {
            isRotating = false;
            rotateBg.SetActive(false);
        }
        else
        {
            isRotating = true;
            rotateBg.SetActive(true);
        }
    }

    public void pressedList()
    {
        if (isList)
        {
            isList = false;
            listBg.SetActive(false);
        }
        else
        {
            isList = true;
            listBg.SetActive(true);
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
