using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/HealthShopUpgrade", fileName = "HealthShopUpgrade", order = 0)]
public class HealthShopUpgrade : BaseUpgradeUIData
{
    public int DeltaHealth;

    public override void OnBuyUpgrade()
    {
        G.Player.Health.MaxHealth += DeltaHealth;
    }
}

