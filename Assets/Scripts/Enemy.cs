using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform targetTransform;
    private Rigidbody2D rigidbody2d;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    public static Enemy Create(Vector3 position)
    {
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private void Start()
    {
        HandleTargeting();
        HandleMovement();
    }

    private void Update()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTarget();
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        // When the building is destroyed, ignore
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;
            float moveSpeed = 6f;

            rigidbody2d.linearVelocity = moveDir * moveSpeed;
        }
        else
        {
            rigidbody2d.linearVelocity = Vector2.zero;

            targetTransform = null; // Clear invalid target
            LookForTarget(); // Immediately look for a new target
        }
    }

    private void HandleTargeting()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        Building hqBuilding = BuildingManager.Instance.GetHqBuilding();
        if (hqBuilding != null && hqBuilding.gameObject != null)
        {
            targetTransform = hqBuilding.transform;
        }

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();

            healthSystem.Damage(10);
            Destroy(gameObject);
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
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                // It is a building


                // If Currently no target then choose the first one
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    // Find the closer target on the map
                    if (
                        Vector3.Distance(transform.position, building.transform.position)
                        < Vector3.Distance(transform.position, targetTransform.position)
                    )
                    {
                        targetTransform = building.transform;
                    }
                }
            }
        }
        if (targetTransform == null)
        {
            // Found no target then force enemy to find the HQ building
            Building hqBuilding = BuildingManager.Instance.GetHqBuilding();
            if (hqBuilding != null && hqBuilding.gameObject != null)
            {
                targetTransform = hqBuilding.transform;
            }
        }
    }
}
