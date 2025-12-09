using UnityEngine;

public class HealthShopUpgrade : BaseUpgradeUIData
{
    void Start()
    {
        OnBuy += OnBuyUpgrade;
    }

    void OnBuyUpgrade()
    {
        Debug.Log("купили макс хп");
    }
}

