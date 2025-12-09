public class MedkitSpawnShopUpgrade : BaseUpgradeUIData
{
    void Start()
    {
        OnBuy += OnBuyUpgrade;
    }

    void OnBuyUpgrade()
    {
        G.PlayerStats.MedkitSpawnUnlock = true;
    }
}