using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/MultShot", fileName = "MultShot", order = 0 )]
public class MultShot : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.MultShotUnlock = true;
        G.PlayerStats.MultShotBulletCount += 1;
    }
}