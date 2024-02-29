using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAimTarget : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CinemachineFreeLook cinemachine;
    [SerializeField] private LayerMask rayLayer;

    void Start()
    {
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 20, rayLayer))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = _camera.transform.position + _camera.transform.forward * 20;
        }
    }
}
