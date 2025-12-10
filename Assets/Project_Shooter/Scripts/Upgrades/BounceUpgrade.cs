using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/BounceUpgrade", fileName = "BounceUpgrade", order = 0 )]
public class BounceUpgrade : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.BounceUpgrade = true;
    }
}