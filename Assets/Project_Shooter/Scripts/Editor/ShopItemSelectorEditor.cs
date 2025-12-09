using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Указываем, что это класс-редактор для компонента ShopItemSelector
[CustomEditor(typeof(ShopItemSelector))]
public class ShopItemSelectorEditor : Editor
{
    // Этот метод будет вызываться при нажатии кнопки в Инспекторе
    public override void OnInspectorGUI()
    {
        // Отрисовка стандартных полей (SerializedProperty)
        DrawDefaultInspector();

        // Получаем ссылку на текущий объект (скрипт ShopItemSelector)
        ShopItemSelector shopSelector = (ShopItemSelector)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Автоматическое заполнение списка", EditorStyles.boldLabel);

        // Создаем кнопку в Инспекторе
        if (GUILayout.Button("Заполнить список Upgrades"))
        {
            FindAllUpgrades(shopSelector);
        }
    }

    private static void FindAllUpgrades(ShopItemSelector selector)
    {
        // 1. Получаем все пути (GUID) к ресурсам (Assets)
        // Ищем только те, которые имеют тип BaseUpgradeUIData
        string[] guids = AssetDatabase.FindAssets("t:BaseUpgradeUIData");
        
        List<BaseUpgradeUIData> foundUpgrades = new List<BaseUpgradeUIData>();

        // 2. Итерируемся по найденным GUID
        foreach (string guid in guids)
        {
            // Получаем путь к ресурсу из GUID
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            
            // Загружаем ресурс по пути и пытаемся преобразовать его в BaseUpgradeUIData
            BaseUpgradeUIData upgrade = AssetDatabase.LoadAssetAtPath<BaseUpgradeUIData>(assetPath);

            if (upgrade != null)
            {
                foundUpgrades.Add(upgrade);
            }
        }
        
        // 3. Присваиваем найденный список переменной в целевом скрипте
        selector.upgrades = foundUpgrades;
        
        // Сообщаем Unity, что объект изменился, чтобы сохранить изменения
        EditorUtility.SetDirty(selector); 
        
        Debug.Log($"[ShopItemSelectorEditor] Успешно найдено и добавлено {foundUpgrades.Count} объектов BaseUpgradeUIData.");
    }
}