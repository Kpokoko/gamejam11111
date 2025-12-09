using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopItemSelector : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject Cursor;
    [SerializeField] private List<EnemyPreset> enemies;
    private List<GameObject> _shopItems = new();
    private List<GameObject> _enemiesCards = new();
    private List<GameObject> _controls = new();
    private int _shopItemsIndexator = 0;
    private int _enemiesCardsIterator = 0;
    private int _controlsIndexator = 0;
    public Mode Mode;
    
    void Awake()
    {
        var upgradesContainer = shopCanvas.transform.Find("UpgradesContainer").gameObject;
        var enemiesCardsContainer = shopCanvas.transform.Find("EnemiesCardsContainer").gameObject;
<<<<<<< Updated upstream
        foreach (Transform upgrade in upgradesContainer.transform)  
=======
        var controlsContainer = shopCanvas.transform.Find("ShopControlsContainer").gameObject;
        foreach (Transform upgrade in upgradesContainer.transform)
        {
>>>>>>> Stashed changes
            _shopItems.Add(upgrade.gameObject);
            UpdateCost(upgrade);
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
            card.GetComponent<EnemyCard>().ResetCard();
        Cursor.transform.position = _controls[0].transform.position;
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
            var upgradeData = _shopItems[_shopItemsIndexator].GetComponentInChildren<BaseUpgradeUIData>();
            Debug.Log(G.PlayerStats is null);
            if (upgradeData.TryBuy(G.PlayerStats.GemCount, out var newMoney))
            {
                G.PlayerStats.GemCount = newMoney;
                UpdateCost(_shopItems[_shopItemsIndexator].transform);
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

    private void UpdateCost(Transform upgrade)
    {
        var upgradeData = upgrade.GetChild(0).gameObject.GetComponent<BaseUpgradeUIData>();
        upgrade.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
            upgradeData.UpgradeCosts[upgradeData.CurrentUpgrade].ToString();
    }
}
