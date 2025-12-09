using System.Collections.Generic;
using Shooter.Gameplay;
using UnityEngine;

public class ShopItemSelector : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject Cursor;
    private List<GameObject> _shopItems = new List<GameObject>();
    private int indexator = 0;
    private int money;
    
    void OnEnable()
    {
        var upgradesContainer = shopCanvas.transform.Find("UpgradesContainer").gameObject;
        foreach (Transform upgrade in upgradesContainer.transform)
        {
            _shopItems.Add(upgrade.gameObject);
        }
        indexator = 0;
        Cursor.transform.position = _shopItems[indexator].transform.position;
        money = PlayerControl.MainPlayerController is null ? 100 : PlayerControl.MainPlayerController.m_GemCount;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (indexator > 0)
            {
                --indexator;
                Cursor.transform.position = _shopItems[indexator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (indexator < _shopItems.Count - 1)
            {
                ++indexator;
                Cursor.transform.position = _shopItems[indexator].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var upgradeData = _shopItems[indexator].GetComponent<BaseUpgradeUIData>();
            if (upgradeData.TryBuy(money, out var newMoney))
            {
                money = newMoney;
                Debug.Log("купили");
            }
            else
                Debug.Log("не купили");
        }
    }
}
