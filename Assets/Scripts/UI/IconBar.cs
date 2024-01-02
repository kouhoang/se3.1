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

    public void pressedDelete()
    {
        if (isRotating)
        {
            isRotating = false;
            rotateBg.SetActive(false);
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
        Vector3 axises = transform.position;
        if (isHid)
        {
            transform.position = new Vector3(-60, axises.y, axises.z);
        }
        else
        {
            transform.position = new Vector3(60, axises.y, axises.z);
        }
    }
}
