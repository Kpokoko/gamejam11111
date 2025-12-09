using System;
using UnityEngine;

public class BaseUpgradeUIData : MonoBehaviour
{
    public int[] UpgradeCosts;
    public int CurrentUpgrade = 0;
    public Action OnBuy;

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
}
