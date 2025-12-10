using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/FastShotShopUpgrade", fileName = "FastShotShopUpgrade", order = 0)]
public class FastShotShopUpgrade : BaseUpgradeUIData
{

    public override void OnBuyUpgrade()
    {
        G.Player.Weapons[0].FireDelay = 0.15f;
    }
}