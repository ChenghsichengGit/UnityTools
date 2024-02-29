using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    private Projectile _projectile;
    private Vector3 targetPos;

    [SerializeField] private float shootTime;
    private float timer;
    private bool isShoot;

    void Update()
    {
        if (GameManager.GameState != GameState.Ongoing)
            return;

        if (isShoot)
            return;

        timer += Time.deltaTime;

        if (timer >= shootTime)
        {
            _projectile = Instantiate(projectile, transform.position, Quaternion.identity);
            _projectile.SetTarget(targetPos);
            isShoot = true;
        }
    }

    public void SetTarget(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
