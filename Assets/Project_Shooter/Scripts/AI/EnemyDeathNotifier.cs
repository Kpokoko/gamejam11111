using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathNotifier : MonoBehaviour
{
    public UnityEvent OnEnemyDeath = new UnityEvent();

    public void NotifyDeath()
    {
        OnEnemyDeath.Invoke();
        Destroy(gameObject); 
    }
}