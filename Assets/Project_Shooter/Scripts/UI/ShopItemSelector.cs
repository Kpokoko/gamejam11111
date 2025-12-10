using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSelector : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject Cursor;
    [SerializeField] private List<EnemyPreset> enemies;
    [SerializeField] private GameObject UpgradePrefab;
    public GameObject UpgradesContainer;
    public List<BaseUpgradeUIData> upgrades;
    
    private List<GameObject> _shopItems = new();
    private List<GameObject> _enemiesCards = new();
    private List<GameObject> _controls = new();
    private int _shopItemsIndexator;
    private int _enemiesCardsIterator;
    private int _controlsIndexator;
    public Mode Mode;
    private TextMeshProUGUI _text;
    public TextMeshProUGUI Description;
    
    void Awake()
    {
        var enemiesCardsContainer = shopCanvas.transform.Find("EnemiesCardsContainer").gameObject;
        var controlsContainer = shopCanvas.transform.Find("ShopControlsContainer").gameObject;
        var moneyInfo = shopCanvas.transform.Find("MoneyInfo").gameObject;
        _text = moneyInfo.GetComponentInChildren<TextMeshProUGUI>();
        foreach (var upgrade in upgrades)
        {
            if (!PlayerProgress.Upgrades.ContainsKey(upgrade))
                PlayerProgress.Upgrades.Add(upgrade, new UpgradeRuntimeData());
            var go = Instantiate(UpgradePrefab, UpgradesContainer.transform, false);
            _shopItems.Add(go);
            upgrade.Subscribe();
            var view = go.GetComponent<ShopItemView>();
            view.image.sprite = upgrade.Icon;
            view.upgradeData = upgrade;
            UpdateCost(upgrade, view);
        }
        foreach (Transform enemyCard in enemiesCardsContainer.transform)
            _enemiesCards.Add(enemyCard.gameObject);
        foreach (Transform control in controlsContainer.transform)
            _controls.Add(control.gameObject);
        Debug.Log(_controls.Count);
        _shopItemsIndexator = 0;
        _enemiesCardsIterator = 0;
        _controlsIndexator = 0;
        Cursor.transform.position = _controls[_controlsIndexator].transform.position;
        Mode = Mode.Controls;
    }
    
    void OnEnable()
    {
        AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopTheme();
        foreach (var card in _enemiesCards)
        {
            var enemyCard = card.GetComponent<EnemyCard>();
            if (enemyCard.IsFlipped)
            {
                enemyCard.ResetCard();
                G.PlayerStats.GemCount += enemyCard.CardPrize;
            }
        }

        _text.text = G.PlayerStats.GemCount.ToString();
        ResizeCursor(_controls, 0);
        Description.text = "Выбери усиление";
        _controlsIndexator = 0;
        Debug.Log(G.PlayerStats.GemCount);
    }
    
    void Update()
    {
        switch (Mode)
        {
            case Mode.Controls:
                ProcessControls();
                break;
            case Mode.Upgrades:
                ProcessUpgrades();
                break;
            case Mode.Enemies:
                ProcessEnemiesCards();
                break;
        }
    }

    private void ProcessControls()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_controlsIndexator > 0)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                --_controlsIndexator;
                ResizeCursor(_controls, _controlsIndexator);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_controlsIndexator < _controls.Count - 1)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                ++_controlsIndexator;
                ResizeCursor(_controls, _controlsIndexator);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
            if (_controlsIndexator == 2)
            {
                var pickedEnemies = new List<EnemyPreset>(_enemiesCards
                    .Where(x => x.GetComponent<EnemyCard>().IsFlipped)
                    .Select(x => x.GetComponent<EnemyCard>().EnemySO));
                if (pickedEnemies.Count == 0)
                {
                    Debug.LogWarning("НЕ ВЫБРАНЫ ВРАГИ");
                    return;
                }
                G.LevelController.EnemyPresets = pickedEnemies;
                G.LevelController.StartLevel();
                return;
            }
            var temp = _controls[_controlsIndexator];
            Mode = temp.GetComponent<ModeWrapper>().Mode;
            if (Mode is Mode.Upgrades)
            {
                ResizeCursor(_shopItems, _shopItemsIndexator);
                Description.text = _shopItems[_shopItemsIndexator].GetComponent<ShopItemView>().upgradeData.Description;
            }
            else if (Mode is Mode.Enemies)
            {
                ResizeCursor(_shopItems, _shopItemsIndexator);
                Cursor.transform.position = _enemiesCards[_enemiesCardsIterator].transform.position;
            }
        }
    }

    void ProcessUpgrades()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_shopItemsIndexator > 0)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                --_shopItemsIndexator;
                ResizeCursor(_shopItems, _shopItemsIndexator);
                Description.text = _shopItems[_shopItemsIndexator].GetComponent<ShopItemView>().upgradeData.Description;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_shopItemsIndexator > 2)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                _shopItemsIndexator -= 3;
                ResizeCursor(_shopItems, _shopItemsIndexator);
                Description.text = _shopItems[_shopItemsIndexator].GetComponent<ShopItemView>().upgradeData.Description;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_shopItemsIndexator < _shopItems.Count - 1)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                ++_shopItemsIndexator;
                ResizeCursor(_shopItems, _shopItemsIndexator);
                Description.text = _shopItems[_shopItemsIndexator].GetComponent<ShopItemView>().upgradeData.Description;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_shopItemsIndexator < _shopItems.Count - 3)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                _shopItemsIndexator += 3;
                ResizeCursor(_shopItems, _shopItemsIndexator);
                Description.text = _shopItems[_shopItemsIndexator].GetComponent<ShopItemView>().upgradeData.Description;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
            var upgradeView = _shopItems[_shopItemsIndexator].GetComponent<ShopItemView>();
            var upgradeData = upgradeView.upgradeData;
            var runtime = PlayerProgress.Upgrades[upgradeData];

            if (runtime.CurrentUpgrade < upgradeData.UpgradeCosts.Length)
            {
                var cost = upgradeData.UpgradeCosts[runtime.CurrentUpgrade];

                if (G.PlayerStats.GemCount >= cost)
                {
                    AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayBuySound();
                    G.PlayerStats.GemCount -= cost;
                    ++runtime.CurrentUpgrade;
                    upgradeData.OnBuy?.Invoke();
                    UpdateCost(upgradeData, upgradeView);
                    _text.text = G.PlayerStats.GemCount.ToString();
                    Debug.Log("купили");
                }
                else
                {
                    Debug.Log("не купили: мало денег");
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.X))
            ReturnToControls();
    }

    void ProcessEnemiesCards()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_enemiesCardsIterator > 0)
                {
                    AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                    --_enemiesCardsIterator;
                    ResizeCursor(_enemiesCards, _enemiesCardsIterator);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_enemiesCardsIterator > 2)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                _enemiesCardsIterator -= 3;
                ResizeCursor(_enemiesCards, _enemiesCardsIterator);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_enemiesCardsIterator < _enemiesCards.Count - 1)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                ++_enemiesCardsIterator;
                ResizeCursor(_enemiesCards, _enemiesCardsIterator);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_enemiesCardsIterator < _enemiesCards.Count - 3)
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
                _enemiesCardsIterator += 3;
                ResizeCursor(_enemiesCards, _enemiesCardsIterator);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
            var enemyCard = _enemiesCards[_enemiesCardsIterator].GetComponent<EnemyCard>();
            if (enemyCard.TryFlip())
            {
                AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayFlipCard();
                var enemySO = enemies[Random.Range(0, enemies.Count)];
                enemyCard.SetEnemySO(enemySO);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
            ReturnToControls();
    }

    private void ReturnToControls()
    {
        AudioManager.Instance.gameObject.GetComponent<AudioPlayer>().PlayShopNavigation();
        Mode = Mode.Controls;
        ResizeCursor(_controls, _controlsIndexator);
        Description.text = "Выбери усиление";
    }

    private void UpdateCost(BaseUpgradeUIData upgrade, ShopItemView view)
    {
        var runtime = PlayerProgress.Upgrades[upgrade];
        if (runtime.CurrentUpgrade < upgrade.UpgradeCosts.Length)
            view.text.text = upgrade.UpgradeCosts[runtime.CurrentUpgrade].ToString();
        else
        {
            view.SoldOutImage.SetActive(true);
            view.text.text = "Выкуплено";
        }
    }

    private void ResizeCursor(List<GameObject> list, int indexator)
    {
        var targetRT = list[indexator].GetComponent<RectTransform>();
        var cursorRT = Cursor.GetComponent<RectTransform>();

        Cursor.transform.position = targetRT.transform.position;

        // РЕАЛЬНОЕ изменение размера
        cursorRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetRT.rect.width);
        cursorRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetRT.rect.height);
    }
}
