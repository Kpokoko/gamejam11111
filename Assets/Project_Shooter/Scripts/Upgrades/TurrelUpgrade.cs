using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/TurrelUpgrade", fileName = "TurrelUpgrade", order = 0 )]
public class TurrelUpgrade : BaseUpgradeUIData
{
    public override void OnBuyUpgrade()
    {
        G.PlayerStats.TurrelUnlock = true;
    }
}