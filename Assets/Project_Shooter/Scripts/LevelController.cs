using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public Transform PlayerSpawnPoint;
    public Transform ArenaSize;
    public float SpawnMedkitInterval = 30f;
    public List<EnemyPreset> EnemyPresets;

    public UnityEvent AllEnemyDead;
    public UnityEvent StartWave;

    public GameObject MedkitPrefab; 
    public GameObject TurrelPrefab;

    public bool isLevelRunning;

    private int _enemiesAliveCount;
    private bool _isSpawningFinished;

    private List<GameObject> AllSpawned = new();

    [Header("DAY SYSTEM")]
    public int DayNumber = 0;          // День начинается с 1

    [Header("ENEMY SPAWN SETTINGS")]
    public float baseInterval = 2f;    // Интервал на 1-й день (твой запрос)
    [Range(0.5f, 0.99f)] public float factor = 0.85f;
    public float minInterval = 0.4f;   // Ограничение, чтобы не было 100 врагов/с
    [Range(0f, 0.5f)] public float jitter = 0.1f;   // Случайность для естественности

    private void Awake()
    {
        G.LevelController = this;
    }

    public void StartLevel()
    {
        foreach (var go in AllSpawned)
            Destroy(go);

        AllSpawned.Clear();

        DayNumber++;
        

        isLevelRunning = true;

        G.Player.transform.position = PlayerSpawnPoint.position;

        StartCoroutine(SpawnEnemiesRoutine());

        if (G.PlayerStats.MedkitSpawnUnlock)
            StartCoroutine(SpawnMedkitRoutine());

        if (G.PlayerStats.TurrelUnlock)
        {
            var t = Instantiate(TurrelPrefab, PlayerSpawnPoint.position, Quaternion.identity);
            AllSpawned.Add(t);
        }
    }

    private IEnumerator SpawnMedkitRoutine()
    {
        while (isLevelRunning)
        {
            yield return new WaitForSeconds(SpawnMedkitInterval);

            var randomSpawnPos = GetRandomPositionInBounds();

            var medkitObj = Instantiate(MedkitPrefab, randomSpawnPos, Quaternion.identity);
            medkitObj.name = $"{MedkitPrefab.name} (Spawned)";
            AllSpawned.Add(medkitObj);
        }
    }

    // ★★★ ГЛАВНАЯ ФОРМУЛА ★★★
    private float GetSpawnInterval()
    {
        float interval = baseInterval * Mathf.Pow(factor, Mathf.Max(0, DayNumber - 1));
        interval = Mathf.Max(minInterval, interval);

        // добавляем джиттер (± j %)
        float offset = interval * jitter * Random.Range(-1f, 1f);
        return interval + offset;
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

        _enemiesAliveCount = shuffledSpawnQueue.Count;

        Debug.Log($"Начало спавна (День {DayNumber}). Всего врагов: {_enemiesAliveCount}");

        foreach (GameObject enemyPrefab in shuffledSpawnQueue)
        {
            var randomSpawnPos = GetRandomPositionInBounds();

            var enemyObj = Instantiate(enemyPrefab, randomSpawnPos, Quaternion.identity);
            enemyObj.name = $"{enemyPrefab.name} (Spawned)";
            AllSpawned.Add(enemyObj);

            // ★★★ ВАЖНО! Используем динамический интервал ★★★
            yield return new WaitForSeconds(GetSpawnInterval());
        }

        Debug.Log("Спавн всех врагов завершен.");
        _isSpawningFinished = true;

        CheckWinCondition();
    }

    public void OnEnemyDied()
    {
        _enemiesAliveCount--;
        Debug.Log($"Враг умер. Осталось: {_enemiesAliveCount}");
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (_isSpawningFinished && _enemiesAliveCount <= 0)
        {
            Debug.Log("Все враги уничтожены. ДЕНЬ ПРОЙДЕН.");
            isLevelRunning = false;
            StartCoroutine(EndWave());
        }
    }

    IEnumerator EndWave()
    {
        yield return new WaitForSeconds(0.25f);
        if(DayNumber == 1)
            G.PlayerStats.GemCount+=100;
        AllEnemyDead.Invoke();
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
            int k = Random.Range(0, n);
            n--;
            (allEnemies[n], allEnemies[k]) = (allEnemies[k], allEnemies[n]);
        }

        return allEnemies;
    }

    private Vector3 GetRandomPositionInBounds()
    {
        if (!ArenaSize)
            return Vector3.zero;

        var center = ArenaSize.position;
        // Используем halfScale для вычисления границ
        var halfScale = ArenaSize.localScale * 0.5f;

        // Ваш код был близок, но вычисление min/max можно сделать более явным:
    
        // Вычисляем минимальные и максимальные координаты
        float minX = center.x - halfScale.x;
        float maxX = center.x + halfScale.x;
    
        float minZ = center.z - halfScale.z;
        float maxZ = center.z + halfScale.z;

        // Генерируем случайную позицию
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);

        return new Vector3(x, center.y, z);
    }
}
