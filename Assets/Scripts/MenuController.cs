using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private MenuPage menuUI;
    // Update is called once per frame
    public void menuOnOff()
    {
        if (menuUI.isActiveAndEnabled == false)
        {
            menuUI.Show();
        }
        else
        {
            menuUI.Hide();
        }
    }

    public void test()
    {
        menuUI.test();
    }
}
