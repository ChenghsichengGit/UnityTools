using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLiveTime : MonoBehaviour
{
    [SerializeField] private float liveTime;

    void Start()
    {
        Destroy(gameObject, liveTime);
    }
}
