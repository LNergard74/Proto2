/*****************************************************************************
// File Name :         CameraPos.cs
// Author :            Lorien Nergard
// Creation Date :     February 16th, 2024
//
// Brief Description : Controls the camera location
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
