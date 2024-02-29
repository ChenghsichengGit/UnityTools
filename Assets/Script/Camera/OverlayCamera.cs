using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayCamera : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Camera _camera;

    void Update()
    {
        _camera.fieldOfView = targetCamera.fieldOfView;
    }
}
