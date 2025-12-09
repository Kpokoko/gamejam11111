using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCard : MonoBehaviour
{
    public bool IsFlipped = false;
    private Transform _cardTransform;
    [SerializeField] private float flipSpeed = 300f;
    [SerializeField] private GameObject RewardTextGO;
    [SerializeField] private GameObject AmountTextGO;
    [SerializeField] private Sprite BaseCardSprite;
    [SerializeField] private Image CardImage;
    public EnemyPreset EnemySO;

    private void Awake()
    {
        _cardTransform = transform;
    }

    public void ResetCard()
    {
        CardImage.sprite = BaseCardSprite;
        RewardTextGO.GetComponent<TextMeshProUGUI>().text = "";
        AmountTextGO.GetComponent<TextMeshProUGUI>().text = "";
        IsFlipped = false;
    }

    public bool TryFlip()
    {
        if (IsFlipped)
            return false;

        StartCoroutine(Flip());
        return true;
    }

    public void SetEnemySO(EnemyPreset enemySO) => this.EnemySO = enemySO;

    private IEnumerator Flip()
    {
        IsFlipped = true;
        var angle = 180f;

        while (angle > 0f)
        {
            var step = flipSpeed * Time.deltaTime;
            angle -= step;
            var finalAngle = Mathf.Max(angle, 0f);

            _cardTransform.localRotation = Quaternion.Euler(0f, finalAngle, 0f);

            if (angle <= 90f && angle + step > 90f)
            {
                Debug.Log("=== CARD AT 90 DEGREES ===");
                Debug.Log(EnemySO is null);
                CardImage.sprite = EnemySO.Sprite;
                RewardTextGO.GetComponent<TextMeshProUGUI>().text = EnemySO.Reward.ToString();
                AmountTextGO.GetComponent<TextMeshProUGUI>().text = EnemySO.Count.ToString();
            }

            yield return null;
        }
    }
}