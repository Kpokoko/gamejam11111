using UnityEngine;

public class HealthShopUpgrade : BaseUpgradeUIData
{
    public int DeltaHealth;
    
    void Start()
    {
        OnBuy += OnBuyUpgrade;
    }

    void OnBuyUpgrade()
    {
        G.Player.Health.MaxHealth += DeltaHealth;
    }
}

