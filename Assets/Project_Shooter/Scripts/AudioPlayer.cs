using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip MenuTheme;
    public AudioClip FightTheme;
    public AudioClip ShopTheme;
    
    public AudioClip ShopNavigation;
    public AudioClip BuyUpgrade;
    public AudioClip FlipCard;
    public AudioClip PickupCoin;

    void Start()
    {
        PlayMenuTheme();
    }

    public void PlayMenuTheme()
    {
        
        AudioManager.Instance.PlayMusic(MenuTheme);
    }

    public void PlayFightTheme()
    {
        AudioManager.Instance.PlayMusic(FightTheme);
    }

    public void PlayShopTheme()
    {
        AudioManager.Instance.PlayMusic(ShopTheme);
    }

    public void PlayShopNavigation()
    {
        AudioManager.Instance.PlaySFX(ShopNavigation);
    }

    public void PlayBuySound()
    {
        AudioManager.Instance.PlaySFX(BuyUpgrade);
    }
    
    public void PlayFlipCard()
    {
        AudioManager.Instance.PlaySFX(FlipCard);
    }
    
    public void PlayPickupCoin(float volume)
    {
        AudioManager.Instance.PlaySFX(PickupCoin, volume);
    }
}