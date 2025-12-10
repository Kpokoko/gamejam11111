using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgradeUIData : ScriptableObject
{
    public Sprite Icon;
    public string Description;
    public int[] UpgradeCosts;
    public Action OnBuy;
    
    public void Subscribe()
    {
        OnBuy += OnBuyUpgrade;
    }

    public virtual void OnBuyUpgrade()
    {
        
    }
}
[Serializable]
public class UpgradeRuntimeData
{
    public int CurrentUpgrade;
}

public static class PlayerProgress
{
    public static Dictionary<BaseUpgradeUIData, UpgradeRuntimeData> Upgrades = new();
}

