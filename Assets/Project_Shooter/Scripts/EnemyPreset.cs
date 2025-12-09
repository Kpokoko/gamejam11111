using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Game/EnemyType")]
public class EnemyPreset : ScriptableObject
{
    public Sprite Sprite;
    public GameObject EnemyPrefab;
    public int Count;
    public int Reward;
}