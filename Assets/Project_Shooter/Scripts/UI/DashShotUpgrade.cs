using UnityEngine;

public class DashShotUpgrade : BaseUpgradeUIData
{
    void Start()
    {
        OnBuy += OnBuyUpgrade;
    }

    void OnBuyUpgrade()
    {
        Debug.Log("купили дэш");
    }
}


