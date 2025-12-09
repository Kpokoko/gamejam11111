using UnityEngine;
using System.Collections; 
using System;

public class WarningEnemySpawn : MonoBehaviour
{
    public float Timer = 2f;
    public GameObject SpawnPrefab;

    private float _initialTimer;
    private Renderer _renderer; 
    private float _timeSinceLastToggle = 0f;

    private void Start()
    {
        _initialTimer = Timer; 
        _renderer = GetComponentInChildren<Renderer>();
        StartCoroutine(SpawnAfter());
    }

    private void Update()
    {
        // if (_renderer)
        // {
        //     float remainingTime = Mathf.Max(0, Timer);
        //     float progress = 1f - (remainingTime / _initialTimer); 
        //
        //     float maxDelay = 0.5f; 
        //     float minDelay = 0.05f; 
        //     
        //     float toggleDelay = Mathf.Lerp(maxDelay, minDelay, progress);
        //
        //     _timeSinceLastToggle += Time.deltaTime;
        //
        //     if (_timeSinceLastToggle >= toggleDelay)
        //     {
        //         _renderer.enabled = !_renderer.enabled;
        //         _timeSinceLastToggle = 0f;
        //     }
        // }
        
        Timer -= Time.deltaTime;
    }

    public IEnumerator SpawnAfter()
    {
        while (Timer > 0)
        {
            yield return null; 
        }
        
        if (_renderer != null)
        {
            _renderer.enabled = true;
        }

        Instantiate(SpawnPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}