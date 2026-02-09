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

    // ОСТАВИЛ, но теперь не используется (чтобы не ломать твои настройки в инспекторе)
    [SerializeField] private float _shrinkScale = 0.3f;
    [SerializeField] private float _shrinkDuration = 1.5f;

    [SerializeField] private float _moveUpDuration = 1.5f;
    [SerializeField] private float _moveUpPixels = 60f;

    [Header("New")]
    [SerializeField] private float _spawnDownPixels = 25f; // 1) появляться ниже (UI-пиксели)
    [SerializeField] private float _fadeDuration = 1.5f;   // 2) время затухания (по умолчанию как move/shrink было)
    [SerializeField] private float _speedMultiplier = 1.25f; // 3) +25% быстрее

    public RectTransform RectTransform { get; private set; }

    private Vector3 _baseScale;
    private Sequence _seq;

    private void Awake()
    {
        RectTransform = (RectTransform)transform;
        _baseScale = RectTransform.localScale;

        // если _fadeDuration не настроен — пусть совпадает с движением
        if (_fadeDuration <= 0f) _fadeDuration = _moveUpDuration;
    }

    public void Play(float value, EffectType type, Action onComplete)
    {
        _seq?.Kill();

        _text.gameObject.SetActive(true);
        _text.text = $"{value:0}";

        _damageIcon.gameObject.SetActive(type == EffectType.Damage);
        _healIcon.gameObject.SetActive(type == EffectType.Heal);

        Vector2 startPos = RectTransform.anchoredPosition + Vector2.down * _spawnDownPixels;
        Vector2 endPos = startPos + Vector2.up * _moveUpPixels;

        RectTransform.anchoredPosition = startPos;
        RectTransform.localScale = _baseScale;

        SetAlpha(1f);

        float k = 1f / _speedMultiplier;      // +25% быстрее (если _speedMultiplier=1.25)
        float moveT = _moveUpDuration * k;    // время подъёма
        float fadeT = moveT * 0.5f;           // В 2 раза быстрее исчезает

        _seq = DOTween.Sequence();

        // Если хочешь вообще без скейла — удали эти 2 строки
        float popT = _popDuration * k;
        _seq.Append(RectTransform.DOScale(_baseScale * _popScale, popT).SetEase(Ease.OutBack));
        _seq.Append(RectTransform.DOScale(_baseScale, popT * 0.6f).SetEase(Ease.OutQuad));

        // ПАРАЛЛЕЛЬНО: поднимается + тухнет (fade быстрее)
        _seq.Join(RectTransform.DOAnchorPos(endPos, moveT).SetEase(Ease.OutQuad));
        _seq.Join(DOTween.To(GetAlpha, SetAlpha, 0f, fadeT).SetEase(Ease.OutQuad));

        // Важно: коллбек должен сработать после завершения всей секвенции (то есть после движения).
        _seq.OnComplete(() => onComplete?.Invoke());
    }

    private float GetAlpha() => _text != null ? _text.color.a : 1f;

    private void SetAlpha(float a)
    {
        if (_text != null)
        {
            var c = _text.color;
            c.a = a;
            _text.color = c;
        }

        if (_damageIcon != null)
        {
            var c = _damageIcon.color;
            c.a = a;
            _damageIcon.color = c;
        }

        if (_healIcon != null)
        {
            var c = _healIcon.color;
            c.a = a;
            _healIcon.color = c;
        }
    }

    private void OnDestroy() =>
        _seq?.Kill();
}
