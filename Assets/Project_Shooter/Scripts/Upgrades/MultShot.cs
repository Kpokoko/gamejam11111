using UnityEngine;

[CreateAssetMenu(menuName = "Create MultShot", fileName = "MultShot", order = 0 )]
public class MultShot : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.MultShotUnlock = true;
    }
}