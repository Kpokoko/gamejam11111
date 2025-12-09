using System;
using UnityEngine;

public class BaseUpgradeUIData : ScriptableObject
{
    public Sprite Icon;
    public string Description;
    public int[] UpgradeCosts;
    [HideInInspector] public int CurrentUpgrade;
    public Action OnBuy;
    
    public void Subscribe()
    {
        OnBuy += OnBuyUpgrade;
    }

    public bool TryBuy(int money, out int newMoney)
    {
        newMoney = 0;
        if (UpgradeCosts.Length >= CurrentUpgrade + 1 && UpgradeCosts[CurrentUpgrade] <= money)
        {
            newMoney = money - UpgradeCosts[CurrentUpgrade];
            ++CurrentUpgrade;
            Debug.Log($"денег: {newMoney}, ещё таких апгрейдов: {UpgradeCosts.Length - CurrentUpgrade}");
            OnBuy?.Invoke();
            return true;
        }

        return false;
    }

    public virtual void OnBuyUpgrade()
    {
        
    }
}
