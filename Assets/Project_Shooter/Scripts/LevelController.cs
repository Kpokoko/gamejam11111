using System.Collections;
using System.Collections.Generic;
using Shooter.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public Transform ArenaSize; 
    public float SpawnInterval = 4f;
    
    public List<EnemyPreset> EnemyPresets;
    
    public UnityEvent AllEnemyDead; 
    
    // Внутренний счетчик живых врагов
    private int _enemiesAliveCount;
    // Флаг, указывающий, завершился ли спавн
    private bool _isSpawningFinished;
    
    public void Start()
    {
        // EnemyPresets = selectedPresets; // Разблокировать, если получаете список извне
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
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

            var enemy = newEnemyObj.GetOrAddComponent<Enemy>();
            
            if (enemy)
            {
                enemy.EnemyHealth.OnDeath.AddListener(OnEnemyDied);
            }
            else
            {
                Debug.LogError($"На префабе {enemyPrefab.name} отсутствует компонент EnemyDeathNotifier!");
                _enemiesAliveCount--;
            }
            
            yield return new WaitForSeconds(SpawnInterval);
        }

        Debug.Log("Спавн всех врагов завершен.");
        _isSpawningFinished = true; // Устанавливаем флаг, что спавн окончен
        
        CheckWinCondition();
    }
    
    private void OnEnemyDied()
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
        List<GameObject> allEnemies = new List<GameObject>();
        
        foreach (var preset in presets)
        {
            for (int i = 0; i < preset.Count; i++)
            {
                allEnemies.Add(preset.EnemyPrefab);
            }
        }

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
        
        Vector3 center = ArenaSize.position;
        Vector3 halfScale = ArenaSize.localScale / 2f; 
        
        float randomX = Random.Range(center.x - halfScale.x, center.x + halfScale.x);
        float randomZ = Random.Range(center.z - halfScale.z, center.z + halfScale.z);
        
        float fixedY = center.y; 
        
        return new Vector3(randomX, fixedY, randomZ);
    }
}