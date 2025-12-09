
public class DashShotUpgrade : BaseUpgradeUIData
{
    void Start()
    {
        OnBuy += OnBuyUpgrade;
    }

    void OnBuyUpgrade()
    {
        G.PlayerStats.DashUnlock = true;
    }
}