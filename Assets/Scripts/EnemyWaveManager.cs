using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public event EventHandler OnWaveNumberChanged;

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    [SerializeField]
    private List<Transform> spawnPositionTransformList;

    [SerializeField]
    private Transform nextWaveSpawnPositionTransform;

    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    public static EnemyWaveManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        nextWaveSpawnTimer = 3f;
        spawnPosition = spawnPositionTransformList[
            UnityEngine.Random.Range(0, spawnPositionTransformList.Count)
        ].position;

        // Warning user the upcoming wave
        nextWaveSpawnPositionTransform.position = spawnPosition;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;

                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;

            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .2f);
                        Enemy.Create(
                            spawnPosition
                                + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(1f, 10f)
                        );
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[
                                UnityEngine.Random.Range(0, spawnPositionTransformList.Count)
                            ].position;

                            // Warning player the upcoming wave
                            nextWaveSpawnPositionTransform.position = spawnPosition;

                            // Reset spawning Time
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 300 + 3 * waveNumber;

        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
        // Change state
        state = State.SpawningWave;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
