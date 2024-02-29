using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    public void SwitchCamera(string name)
    {
        Animator.CrossFadeInFixedTime("To" + name, 0.1f);
    }
}
