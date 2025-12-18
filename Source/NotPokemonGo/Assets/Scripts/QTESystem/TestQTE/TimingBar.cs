using DG.Tweening;
using QTESystem.TestQTE;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour
{
   [Header("References")]
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform cursor;

    [SerializeField] private Image failLeft;
    [SerializeField] private Image okZone;
    [SerializeField] private Image perfectZone;
    [SerializeField] private Image failRight;

    [Header("Difficulty")]
    [SerializeField] private QTEDifficulty difficulty;

    [Header("Input")]
    [SerializeField] private KeyCode inputKey = KeyCode.Space;

    private Tween cursorTween;
    private float barWidth;

    private float perfectMin;
    private float perfectMax;
    private float okMin;
    private float okMax;

    private bool active;

    private void Start()
    {
        barWidth = bar.rect.width;
        SetupZones();
        StartQTE();
    }

    private void Update()
    {
        if (!active) return;

        if (Input.GetKeyDown(inputKey))
        {
            Evaluate();
        }
    }

    // ---------------------------
    // QTE LOGIC
    // ---------------------------

    public void StartQTE()
    {
        active = true;

        float left = -barWidth / 2f;
        float right = barWidth / 2f;

        cursor.anchoredPosition = new Vector2(left, 0);

        cursorTween?.Kill();
        cursorTween = cursor
            .DOAnchorPosX(right, difficulty.duration)
            .SetEase(Ease.Linear)
            .SetLoops(difficulty.pingPong ? -1 : 1, LoopType.Yoyo);
    }

    public void StopQTE()
    {
        active = false;
        cursorTween?.Kill();
    }

    private void Evaluate()
    {
        StopQTE();

        float pos = GetNormalizedCursorPosition();

        if (pos >= perfectMin && pos <= perfectMax)
        {
            Debug.Log("⭐ PERFECT");
        }
        else if (pos >= okMin && pos <= okMax)
        {
            Debug.Log("✔ OK");
        }
        else
        {
            Debug.Log("❌ FAIL");
        }
    }

    // ---------------------------
    // ZONES
    // ---------------------------

    private void SetupZones()
    {
        float perfectWidth = barWidth * difficulty.perfectSize;
        float okWidth = barWidth * difficulty.okSize;

        // PERFECT
        perfectZone.rectTransform.sizeDelta =
            new Vector2(perfectWidth, bar.rect.height);

        // OK
        okZone.rectTransform.sizeDelta =
            new Vector2(okWidth, bar.rect.height);

        // FAIL
        float failWidth = (barWidth - okWidth) / 2f;
        failLeft.rectTransform.sizeDelta =
            new Vector2(failWidth, bar.rect.height);
        failRight.rectTransform.sizeDelta =
            new Vector2(failWidth, bar.rect.height);

        // Positions
        perfectZone.rectTransform.anchoredPosition = Vector2.zero;
        okZone.rectTransform.anchoredPosition = Vector2.zero;

        failLeft.rectTransform.anchoredPosition =
            new Vector2(-barWidth / 2f + failWidth / 2f, 0);
        failRight.rectTransform.anchoredPosition =
            new Vector2(barWidth / 2f - failWidth / 2f, 0);

        // Normalized windows
        float perfectHalf = difficulty.perfectSize / 2f;
        float okHalf = difficulty.okSize / 2f;

        perfectMin = 0.5f - perfectHalf;
        perfectMax = 0.5f + perfectHalf;

        okMin = 0.5f - okHalf;
        okMax = 0.5f + okHalf;
    }

    private float GetNormalizedCursorPosition()
    {
        float x = cursor.anchoredPosition.x;
        return Mathf.InverseLerp(-barWidth / 2f, barWidth / 2f, x);
    }
}
