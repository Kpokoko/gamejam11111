using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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
    
    void Awake()
    {
        var upgradesContainer = shopCanvas.transform.Find("UpgradesContainer").gameObject;
        var enemiesCardsContainer = shopCanvas.transform.Find("EnemiesCardsContainer").gameObject;
        var controlsContainer = shopCanvas.transform.Find("ShopControlsContainer").gameObject;
        foreach (var upgrade in upgrades)
        {
            var go = Instantiate(UpgradePrefab, UpgradesContainer.transform, false);

            _shopItems.Add(go);
    
            upgrade.Subscribe();
    
            var view = go.GetComponent<ShopItemView>();
            view.image.sprite = upgrade.Icon;
            view.upgradeData =  upgrade;
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
        foreach (var card in _enemiesCards)
        {
            var enemyCard = card.GetComponent<EnemyCard>();
            if (enemyCard.IsFlipped)
            {
                enemyCard.ResetCard();
                G.PlayerStats.GemCount += enemyCard.CardPrize;
            }
        }
        Cursor.transform.position = _controls[0].transform.position;
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
                --_controlsIndexator;
                Cursor.transform.position = _controls[_controlsIndexator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_controlsIndexator < _controls.Count - 1)
            {
                ++_controlsIndexator;
                Cursor.transform.position = _controls[_controlsIndexator].transform.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
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
                Cursor.transform.position = _shopItems[_shopItemsIndexator].transform.position;
            else if (Mode is Mode.Enemies)
                Cursor.transform.position = _enemiesCards[_enemiesCardsIterator].transform.position;
        }
    }

    void ProcessUpgrades()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_shopItemsIndexator > 0)
            {
                --_shopItemsIndexator;
                Cursor.transform.position = _shopItems[_shopItemsIndexator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_shopItemsIndexator > 2)
            {
                _shopItemsIndexator -= 3;
                Cursor.transform.position = _shopItems[_shopItemsIndexator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_shopItemsIndexator < _shopItems.Count - 1)
            {
                ++_shopItemsIndexator;
                Cursor.transform.position = _shopItems[_shopItemsIndexator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_shopItemsIndexator < _shopItems.Count - 3)
            {
                _shopItemsIndexator += 3;
                Cursor.transform.position = _shopItems[_shopItemsIndexator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //var upgradeData = _shopItems[_shopItemsIndexator].GetComponentInChildren<BaseUpgradeUIData>();
            var upgradeView = _shopItems[_shopItemsIndexator].GetComponentInChildren<ShopItemView>();
            var upgradeData = upgradeView.upgradeData;
            if (upgradeData.TryBuy(G.PlayerStats.GemCount, out var newMoney))
            {
                G.PlayerStats.GemCount = newMoney;
                UpdateCost(upgradeData, upgradeView);
                Debug.Log("купили");
            }
            else
                Debug.Log("не купили");
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
                    --_enemiesCardsIterator;
                    Cursor.transform.position = _enemiesCards[_enemiesCardsIterator].transform.position;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_enemiesCardsIterator > 2)
            {
                _enemiesCardsIterator -= 3;
                Cursor.transform.position = _enemiesCards[_enemiesCardsIterator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_enemiesCardsIterator < _enemiesCards.Count - 1)
            {
                ++_enemiesCardsIterator;
                Cursor.transform.position = _enemiesCards[_enemiesCardsIterator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_enemiesCardsIterator < _enemiesCards.Count - 3)
            {
                _enemiesCardsIterator += 3;
                Cursor.transform.position = _enemiesCards[_enemiesCardsIterator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var enemyCard = _enemiesCards[_enemiesCardsIterator].GetComponent<EnemyCard>();
            if (enemyCard.TryFlip())
            {
                var enemySO = enemies[Random.Range(0, enemies.Count)];
                enemyCard.SetEnemySO(enemySO);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
            ReturnToControls();
    }

    private void ReturnToControls()
    {
        Mode = Mode.Controls;
        Cursor.transform.position = _controls[_controlsIndexator].transform.position;
    }

    private void UpdateCost(BaseUpgradeUIData upgrade, ShopItemView view)
    {
        view.text.text = upgrade.UpgradeCosts[upgrade.CurrentUpgrade].ToString();
    }
}
