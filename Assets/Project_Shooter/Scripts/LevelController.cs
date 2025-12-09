using System;
using System.Collections;
using System.Collections.Generic;
using Shooter.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public Transform PlayerSpawnPoint;
    public Transform ArenaSize;
    public float SpawnInterval = 4f;

    public List<EnemyPreset> EnemyPresets;

    public UnityEvent AllEnemyDead;
    public UnityEvent StartWave;

    private int _enemiesAliveCount;
    private bool _isSpawningFinished;

    private void Awake()
    {
        G.LevelController = this;
    }

    public void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        G.Player.transform.position = PlayerSpawnPoint.position;
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        StartWave.Invoke();
        if (EnemyPresets == null || EnemyPresets.Count == 0)
        {
            Debug.LogWarning("Список пресетов врагов пуст.");
            yield break;
        }

        var shuffledSpawnQueue = CreateShuffledQueue(EnemyPresets);

        // Сбрасываем счетчик и устанавливаем общее количество врагов
        _enemiesAliveCount = shuffledSpawnQueue.Count;

        Debug.Log($"Начало спавна. Всего врагов в очереди: {_enemiesAliveCount}");

        foreach (GameObject enemyPrefab in shuffledSpawnQueue)
        {
            Vector3 randomSpawnPos = GetRandomPositionInBounds();

            var newEnemyObj = Instantiate(enemyPrefab, randomSpawnPos, Quaternion.identity);
            newEnemyObj.name = $"{enemyPrefab.name} (Spawned)";


            yield return new WaitForSeconds(SpawnInterval);
        }

        Debug.Log("Спавн всех врагов завершен.");
        _isSpawningFinished = true;

        CheckWinCondition();
    }

    public void OnEnemyDied()
    {
        _enemiesAliveCount--;
        Debug.Log($"Враг умер. Осталось живых: {_enemiesAliveCount}");

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (_isSpawningFinished && _enemiesAliveCount <= 0)
        {
            Debug.Log("ВСЕ ВРАГИ УНИЧТОЖЕНЫ! Вызов события AllEnemyDead.");
            AllEnemyDead.Invoke();
        }
    }


    private List<GameObject> CreateShuffledQueue(List<EnemyPreset> presets)
    {
        var allEnemies = new List<GameObject>();

        foreach (var preset in presets)
            for (int i = 0; i < preset.Count; i++)
                allEnemies.Add(preset.EnemyPrefab);

        int n = allEnemies.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (allEnemies[n], allEnemies[k]) = (allEnemies[k], allEnemies[n]);
        }

        return allEnemies;
    }

    private Vector3 GetRandomPositionInBounds()
    {
        if (ArenaSize == null)
        {
            Debug.LogError("ArenaSize не назначен! Возвращаю (0,0,0).");
            return Vector3.zero;
        }

        var center = ArenaSize.position;
        var halfScale = ArenaSize.localScale / 2f;

        var randomX = Random.Range(center.x - halfScale.x, center.x + halfScale.x);
        var randomZ = Random.Range(center.z - halfScale.z, center.z + halfScale.z);

        var fixedY = center.y;

        return new Vector3(randomX, fixedY, randomZ);
    }
}