using System;
using TMPro;
using UnityEngine;

public class NextEnemyWaveUI : MonoBehaviour
{
    [SerializeField]
    private EnemyWaveManager enemyWaveManager;

    [SerializeField]
    private Transform enemySpawnIndicatorTransform;

    [SerializeField]
    private Transform enemyCloserIndicatorTransform;

    private RectTransform enemySpawnIndicator;
    private RectTransform enemyCloserIndicator;

    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;

    private Camera mainCamera;

    private void Awake()
    {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();

        enemySpawnIndicator = enemySpawnIndicatorTransform.GetComponent<RectTransform>();
        enemyCloserIndicator = enemyCloserIndicatorTransform.GetComponent<RectTransform>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveMangerChanged;

        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    private void EnemyWaveManager_OnWaveMangerChanged(object sender, EventArgs e)
    {
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    private void Update()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f)
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
        }

        // Show indicator
        ShowNextSpawnIndicator(enemySpawnIndicator);
        ShowCloserEnemyIndicator(enemyCloserIndicator);
    }

    private void ShowNextSpawnIndicator(RectTransform indicator)
    {
        // Point the indicator to the incoming wave to notice player
        Vector3 dirToNextSpawnPosition = (
            enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position
        ).normalized;

        indicator.anchoredPosition = dirToNextSpawnPosition * 300f;

        indicator.eulerAngles = new Vector3(
            0,
            0,
            UtilsClass.GetAngleFromVector(dirToNextSpawnPosition)
        );

        // [Optional]: if the camera far away from the spawn point then show the indicator else hide it
        float distanceToNextSpawnPosition = Vector3.Distance(
            enemyWaveManager.GetSpawnPosition(),
            mainCamera.transform.position
        );
        indicator.gameObject.SetActive(
            distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f
        );
    }

    private void ShowCloserEnemyIndicator(RectTransform indicator)
    {
        float targetMaxRadius = 99999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(
            mainCamera.transform.position,
            targetMaxRadius
        );
        Enemy targetEnemy = null;
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

        if (targetEnemy != null)
        {
            // Point the indicator to the incoming wave to notice player
            Vector3 dirToCloserSpawnPosition = (
                targetEnemy.transform.position - mainCamera.transform.position
            ).normalized;

            indicator.anchoredPosition = dirToCloserSpawnPosition * 250f;

            indicator.eulerAngles = new Vector3(
                0,
                0,
                UtilsClass.GetAngleFromVector(dirToCloserSpawnPosition)
            );

            // [Optional]: if the camera far away from the spawn point then show the indicator else hide it
            float distanceToCloserEnemy = Vector3.Distance(
                enemyWaveManager.GetSpawnPosition(),
                mainCamera.transform.position
            );
            indicator.gameObject.SetActive(
                distanceToCloserEnemy > mainCamera.orthographicSize * 1.5f
            );
        }
        else
        {
            // No enemies alive
            indicator.gameObject.SetActive(false);
        }
    }

    private void SetMessageText(string message)
    {
        waveMessageText.SetText(message);
    }

    private void SetWaveNumberText(string message)
    {
        waveNumberText.SetText(message);
    }
}
