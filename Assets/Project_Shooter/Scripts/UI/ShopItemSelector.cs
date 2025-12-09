using System.Collections.Generic;
using Shooter.Gameplay;
using UnityEngine;

public class ShopItemSelector : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject Cursor;
    [SerializeField] private List<EnemyPreset> enemies;
    private List<GameObject> _shopItems = new();
    private List<GameObject> _enemiesCards = new();
    private int _shopIndexator = 0;
    
    void OnEnable()
    {
        var upgradesContainer = shopCanvas.transform.Find("UpgradesContainer").gameObject;
        var enemiesCardsContainer = shopCanvas.transform.Find("EnemiesCardsContainer").gameObject;
        foreach (Transform upgrade in upgradesContainer.transform)
            _shopItems.Add(upgrade.gameObject);
        foreach (Transform enemyCard in enemiesCardsContainer.transform)
            _enemiesCards.Add(enemyCard.gameObject);
        _shopIndexator = 0;
        Cursor.transform.position = _shopItems[_shopIndexator].transform.position;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_shopIndexator > 0)
            {
                --_shopIndexator;
                if (_shopIndexator < _shopItems.Count)
                    Cursor.transform.position = _shopItems[_shopIndexator].transform.position;
                else
                    Cursor.transform.position = _enemiesCards[_shopIndexator % _shopItems.Count].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_shopIndexator < _shopItems.Count + _enemiesCards.Count - 1)
            {
                ++_shopIndexator;
                if (_shopIndexator < _shopItems.Count)
                    Cursor.transform.position = _shopItems[_shopIndexator].transform.position;
                else
                    Cursor.transform.position = _enemiesCards[_shopIndexator % _shopItems.Count].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_shopIndexator < _shopItems.Count)
            {
                var upgradeData = _shopItems[_shopIndexator].GetComponent<BaseUpgradeUIData>();
                if (upgradeData.TryBuy(PlayerControl.MainPlayerController.m_GemCount, out var newMoney))
                {
                    PlayerControl.MainPlayerController.m_GemCount = newMoney;
                    Debug.Log("купили");
                }
                else
                    Debug.Log("не купили");
            }
            else
            {
                var enemyCard = _enemiesCards[_shopIndexator % _enemiesCards.Count].GetComponent<EnemyCard>();
                if (enemyCard.TryFlip())
                {
                    var enemySO = this.enemies[Random.Range(0, this.enemies.Count)];
                    enemyCard.SetEnemySO(enemySO);
                }
            }
        }
    }
}
