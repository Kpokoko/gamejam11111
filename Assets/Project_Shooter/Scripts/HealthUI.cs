using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Sprite filledSprite;
    public Sprite emptySprite;

    public GridLayoutGroup gridLayout;

    public void UpdateHealthDisplay(int currentHealth)
    {
        var maxHealth = 11;
        Image[] healthCells = gridLayout.GetComponentsInChildren<Image>(includeInactive: false);

        if (healthCells.Length != maxHealth)
        {
            Debug.LogError($"Количество ячеек ({healthCells.Length}) не совпадает с максимальным здоровьем ({maxHealth}).");
            return;
        }

        for (int i = 0; i < maxHealth; i++)
        {
            Image cell = healthCells[i];
            
            if (i < currentHealth)
            {
                cell.sprite = filledSprite;
            }
            else
            {
                cell.sprite = emptySprite;
            }
        }
    }
}