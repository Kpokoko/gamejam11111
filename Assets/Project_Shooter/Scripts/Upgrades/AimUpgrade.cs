using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/AimUpgrade", fileName = "AimUpgrade", order = 0 )]
public class AimUpgrade : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.SoftAimActive = true;
    }
}