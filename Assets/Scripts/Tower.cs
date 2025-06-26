using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float shootTimerMax;
    private float shootTimer;

    private Enemy targetEnemy;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;
    private Vector3 projectileSpawnPosition;

    private void Awake()
    {

        projectileSpawnPosition = transform.Find("ProjectileSpawnPosition").position;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer += shootTimerMax;

            if (targetEnemy != null)
            {
                ArrowProjectile.Create(projectileSpawnPosition, targetEnemy);
            }
        }
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTarget();
        }
    }

    private void LookForTarget()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(
            transform.position,
            targetMaxRadius
        );

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                // It is a enemy
                // If Currently no target then choose the first one
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    // Find the closer target on the map
                    if (
                        Vector3.Distance(transform.position, enemy.transform.position)
                        < Vector3.Distance(transform.position, targetEnemy.transform.position)
                    )
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
    }
}
