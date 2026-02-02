using System;
using DG.Tweening;
using Effects;
using Plugins.Demigiant.DOTween.Modules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Units
{
	public class UnitDamageView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _damageText;
		[SerializeField] private Image _damageImage;
		[SerializeField] private Image _healImage;

		[Header("Anim settings")] [SerializeField]
		private float _popScale = 1.1f; // +10%

		[SerializeField] private float _popDuration = 0.4f;
		[SerializeField] private float _shrinkScale = 0.3f; // -70% => 30%
		[SerializeField] private float _shrinkDuration = 1.5f;
		[SerializeField] private float _moveUpDuration = 1.5f;
		[SerializeField] private float _moveUpPixels = 60f; // подбери под свой Canvas

		private IEffectResolver _resolver;

		private Vector3 _textBaseScale;
		private Vector3 _iconBaseScale;
		private Vector2 _textBasePos;
		private Vector2 _iconBasePos;

		private RectTransform _textRt;
		private RectTransform _iconRt;

		private Sequence _seq;

		[Inject]
		private void Initializer(IEffectResolver resolver)
		{
			_resolver = resolver;
			_resolver.EffectApllayed += OnEffectApplayed;
			Debug.Log("UnitDamageView initialized");
		}

		private void Awake()
		{
			_textRt = _damageText.rectTransform;

			_textBaseScale = _textRt.localScale;
			_textBasePos = _textRt.anchoredPosition;

			// Иконка будет меняться (damage/heal), базу позы/скейла возьмём у damageImage как эталон
			_iconRt = _damageImage.rectTransform;
			_iconBaseScale = _iconRt.localScale;
			_iconBasePos = _iconRt.anchoredPosition;
		}

		private void OnDestroy()
		{
			_resolver.EffectApllayed -= OnEffectApplayed;

			_seq?.Kill();
		}

		private void OnEffectApplayed(Unit source, Unit target, float effectValue, EffectInfo effectInfo)
		{
			Debug.Log("OnEffectApplayed");
			_damageText.enabled = true;
			_damageText.text = $"{effectValue}";

			Image activeIcon = effectInfo.Type switch
			{
				EffectType.Damage => _damageImage,
				EffectType.Heal => _healImage,
				_ => throw new ArgumentOutOfRangeException()
			};

			_damageImage.enabled = (activeIcon == _damageImage);
			_healImage.enabled = (activeIcon == _healImage);

			PlayFlyAnim(activeIcon);
		}

		private void PlayFlyAnim(Image activeIcon)
		{
			Debug.Log("PlayFlyAnim");
			// если уже летит — убиваем и сбрасываем в базу (иначе “улетит в космос” от накопления)
			_seq?.Kill();

			var iconRt = activeIcon.rectTransform;

			// Сброс состояния перед новой анимацией
			_textRt.localScale = _textBaseScale;
			_textRt.anchoredPosition = _textBasePos;

			iconRt.localScale = _iconBaseScale;
			iconRt.anchoredPosition = _iconBasePos;

			// (опционально) убедимся что активная иконка включена
			activeIcon.enabled = true;

			Vector3 popTextScale = _textBaseScale * _popScale;
			Vector3 popIconScale = _iconBaseScale * _popScale;

			Vector3 shrinkTextScale = _textBaseScale * _shrinkScale;
			Vector3 shrinkIconScale = _iconBaseScale * _shrinkScale;

			Vector2 textUpPos = _textBasePos + Vector2.up * _moveUpPixels;
			Vector2 iconUpPos = _iconBasePos + Vector2.up * _moveUpPixels;

			_seq = DOTween.Sequence();

			// 1) POP (0.4s) — текст + иконка параллельно
			_seq.Join(_textRt.DOScale(popTextScale, _popDuration).SetEase(Ease.OutBack));
			_seq.Join(iconRt.DOScale(popIconScale, _popDuration).SetEase(Ease.OutBack));

			// 2) SHRINK (1.5s) — текст + иконка параллельно
			_seq.Append(_textRt.DOScale(shrinkTextScale, _shrinkDuration).SetEase(Ease.InQuad));
			_seq.Join(iconRt.DOScale(shrinkIconScale, _shrinkDuration).SetEase(Ease.InQuad));

			// 3) MOVE UP (1.5s) — параллельно со shrink (чтобы получилось “одновременно 1.5s”)
			_seq.Join(_textRt.DOAnchorPos(textUpPos, _moveUpDuration).SetEase(Ease.OutQuad));
			_seq.Join(iconRt.DOAnchorPos(iconUpPos, _moveUpDuration).SetEase(Ease.OutQuad));

			// В конце можно отключать иконку, а текст оставлять/тоже скрывать — как хочешь.
			_seq.OnComplete(() =>
			{
				activeIcon.enabled = false;
				_damageText.enabled = false;
			});
		}
	}
}


