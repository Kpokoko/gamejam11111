using UnityEngine;

public class FastShotShopUpgrade : BaseUpgradeUIData
{
    void Start()
    {
        OnBuy += OnBuyUpgrade;
    }

    void OnBuyUpgrade()
    {
        Debug.Log("купили ускорение стрельбы");
    }
}
