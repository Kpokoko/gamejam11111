using UnityEngine;

[CreateAssetMenu(menuName = "Create MedkitSpawnShopUpgrade", fileName = "MedkitSpawnShopUpgrade", order = 0 )]
public class MedkitSpawnShopUpgrade : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.MedkitSpawnUnlock =  true;
    }
}