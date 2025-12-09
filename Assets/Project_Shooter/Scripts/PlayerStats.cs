using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private void Awake()
    {
        G.PlayerStats = this;
    }

    public int GemCount;
    
    
    public bool SoftAimActive;
    public bool DashUnlock;
    public bool MedkitSpawnUnlock;
}