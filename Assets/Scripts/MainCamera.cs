using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    PortalCameraController[] portals;

    void Awake()
    {
        portals = FindObjectsOfType<PortalCameraController> ();
    }

    void OnPreCull()
    {

      
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].Render();
        }

       

    }
}
