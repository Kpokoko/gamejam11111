
using UnityEngine;

[CreateAssetMenu(menuName = "Create DashShotUpgrade", fileName = "DashShotUpgrade", order = 0)]
public class DashShotUpgrade : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.DashUnlock = true;
    }
}