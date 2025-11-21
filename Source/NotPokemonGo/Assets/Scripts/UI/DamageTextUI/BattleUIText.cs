using System;
using Services.ObjectPools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.DamageTextUI
{
  public class BattleUIText : MonoBehaviour, IPoolabelObject
  {
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private Image _image;
    
    [Header("Move & Shrink")]
    [SerializeField] private float _moveUpDistance = 1f;      // насколько вверх
    [SerializeField] private float _moveDuration = 2f;        // длительность движения
    [SerializeField] private float _shrinkMultiplier = 1.5f;  // уменьшение в 1.5 раза

    [Header("Pop (appear)")]
    [SerializeField] private float _popScaleMultiplier = 1.2f;  // насколько увеличиваем при появлении
    [SerializeField] private float _popDuration = 0.9f;         // общая длительность попа
    
    public event Action<BattleUIText> AnimationEnded;
    private Sequence _sequence;
    public void Initialize(Vector3 position, float targetValue, Sprite sprite)
    {
      transform.position = position;
      _damageText.text = targetValue.ToString();
      _image.sprite = sprite;

      PlayAnimation();
    }

    public void PollableDispose()
    { }
    
    private void PlayAnimation()
    {
      // на всякий случай убиваем старую анимацию (если объект из пула)
      _sequence?.Kill();
      _sequence = DOTween.Sequence();

      Transform root = transform;
      RectTransform textRect = _damageText.rectTransform;
      RectTransform imageRect = _image.rectTransform;

      Vector3 startPos = root.position;
      Vector3 endPos = startPos + Vector3.up * _moveUpDistance;
      float halfPop = _popDuration * 0.5f;
      Vector3 finalRootScale = Vector3.one / _shrinkMultiplier;

      // 1) Движение вверх + общий SHRINK за 2 секунды
      _sequence.Join(
        root.DOMove(endPos, _moveDuration)
          .SetEase(Ease.OutCubic));

      _sequence.Join(
        root.DOScale(finalRootScale, _moveDuration)
          .SetEase(Ease.Linear));

      // 2) POP-анимация для текста и картинки (0.9 сек)

      // увеличиваем
      _sequence.Join(
        textRect.DOScale(_popScaleMultiplier, halfPop)
          .SetEase(Ease.OutQuad));
      _sequence.Join(
        imageRect.DOScale(_popScaleMultiplier, halfPop)
          .SetEase(Ease.OutQuad));

      // затем обратно к 1
      _sequence.Insert(
        halfPop,
        textRect.DOScale(1f, halfPop)
          .SetEase(Ease.InQuad));
      _sequence.Insert(
        halfPop,
        imageRect.DOScale(1f, halfPop)
          .SetEase(Ease.InQuad));

      // когда всё закончилось — вызываем событие
      _sequence.OnComplete(() =>
      {
        AnimationEnded?.Invoke(this);
      });
    }
  }
}