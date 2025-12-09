using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCard : MonoBehaviour
{
    private bool _isFlipped = false;
    private Transform _cardTransform;
    [SerializeField] private float flipSpeed = 300f;
    [SerializeField] private GameObject RewardTextGO;
    [SerializeField] private GameObject AmountTextGO;
    [SerializeField] private Image CardImage;
    private EnemyPreset _enemySO;

    private void Awake()
    {
        _cardTransform = transform;
    }

    public bool TryFlip()
    {
        if (_isFlipped)
            return false;

        StartCoroutine(Flip());
        return true;
    }

    public void SetEnemySO(EnemyPreset enemySO) => this._enemySO = enemySO;

    private IEnumerator Flip()
    {
        _isFlipped = true;
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
                CardImage.sprite = _enemySO.Sprite;
                RewardTextGO.GetComponent<TextMeshProUGUI>().text = _enemySO.Reward.ToString();
                AmountTextGO.GetComponent<TextMeshProUGUI>().text = _enemySO.Count.ToString();
            }

            yield return null;
        }
    }
}