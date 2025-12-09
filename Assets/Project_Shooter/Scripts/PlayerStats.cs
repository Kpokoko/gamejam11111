using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private void Awake()
    {
        G.PlayerStats = this;
    }

    public Dictionary<BaseUpgradeUIData, int> UpgradeData;
    public int GemCount;
}