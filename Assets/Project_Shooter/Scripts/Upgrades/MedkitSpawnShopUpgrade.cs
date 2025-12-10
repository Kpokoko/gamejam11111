using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/MedkitSpawnShopUpgrade", fileName = "MedkitSpawnShopUpgrade", order = 0 )]
public class MedkitSpawnShopUpgrade : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.MedkitSpawnUnlock =  true;
    }
}