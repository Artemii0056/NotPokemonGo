using System;
using DG.Tweening;
using Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CombatText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _damageIcon;
    [SerializeField] private Image _healIcon;

    [Header("Anim")]
    [SerializeField] private float _popScale = 1.1f;
    [SerializeField] private float _popDuration = 0.4f;
    [SerializeField] private float _shrinkScale = 0.3f;
    [SerializeField] private float _shrinkDuration = 1.5f;
    [SerializeField] private float _moveUpDuration = 1.5f;
    [SerializeField] private float _moveUpPixels = 60f;

    public RectTransform RectTransform { get; private set; }
    
    private Vector3 _baseScale;
    private Vector2 _basePos;
    private Sequence _seq;

    private void Awake()
    {
        RectTransform = (RectTransform)transform;
        _baseScale = RectTransform.localScale;
        _basePos = RectTransform.anchoredPosition;
    }

    public void Play(float value, EffectType type, Action onComplete)
    {
        _seq?.Kill();

        _text.gameObject.SetActive(true);
        _text.text = $"{value:0}";

        _damageIcon.gameObject.SetActive(type == EffectType.Damage);
        _healIcon.gameObject.SetActive(type == EffectType.Heal);

        var startPos = RectTransform.anchoredPosition;  
        RectTransform.localScale = _baseScale;

        Vector3 pop = _baseScale * _popScale;
        Vector3 shrink = _baseScale * _shrinkScale;
        Vector2 up = startPos + Vector2.up * _moveUpPixels;

        _seq = DOTween.Sequence();
        _seq.Append(RectTransform.DOScale(pop, _popDuration).SetEase(Ease.OutBack));
        _seq.Append(RectTransform.DOScale(shrink, _shrinkDuration).SetEase(Ease.InQuad));
        _seq.Join(RectTransform.DOAnchorPos(up, _moveUpDuration).SetEase(Ease.OutQuad));
        _seq.OnComplete(() => onComplete?.Invoke());
    }

    private void OnDestroy() => 
        _seq?.Kill();
}

