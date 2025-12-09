using UnityEngine;

public class FastShotShopUpgrade : BaseUpgradeUIData
{
    void Start()
    {
        OnBuy += OnBuyUpgrade;
    }

    void OnBuyUpgrade()
    {
        G.Player.Weapons[0].FireDelay = 0.15f;
    }
}