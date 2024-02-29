using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private TrailRenderer[] trails;
    [SerializeField] private GameObject hitParticle;
    private Vector3 targetPos;
    private Vector3 targetDir;

    [SerializeField] public bool flyUp;
    [SerializeField] public bool flySideways;
    [SerializeField] private float speed;
    [SerializeField] private float frequency;
    [SerializeField] private float strength;
    private float time = 0;

    private void Start()
    {
        targetDir = targetPos - transform.position;
    }

    private void Update()
    {
        if (GameManager.GameState != GameState.Ongoing)
            return;

        time += Time.deltaTime;
        transform.position += targetDir.normalized * speed * Time.deltaTime + CheckFlightDirection() * CalculateSin();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitParticle)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }

            foreach (TrailRenderer trail in trails)
            {
                trail.enabled = false;
            }

            Instantiate(hitParticle, transform.position, Quaternion.identity);

            Debug.Log(other.name);

            Destroy(gameObject, 1);
        }
    }

    public Vector3 CheckFlightDirection()
    {
        if (!flyUp && flySideways)
            return Vector3.zero;

        if (flyUp)
            return Vector3.up;

        if (flySideways)
            return Vector3.right;

        return Vector3.up + Vector3.right;
    }

    public float CalculateSin()
    {
        return Mathf.Sin(frequency * time) * strength * Time.deltaTime;
    }

    public void SetTarget(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
