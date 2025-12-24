using System;
using DG.Tweening;
using QTESystem.TestQTE;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private RectTransform _cursor;

    [SerializeField] private RectTransform _bar;

    [SerializeField] private Image _okZoneImage;
    [SerializeField] private Image _perfectZoneImage;

    private Tween _tween;
    private float _timer;

    [SerializeField] private QteDifficulty Difficulty;

    public Action Fail;
    public Action Ok;
    public Action Perfect;

    public void Play()
    {
        _cursor.gameObject.SetActive(true);
        SetCursor(0);
        StartQte();
    }

    private void StartQte()
    {
        _timer = 0f;

        _tween?.Kill();
        _tween = DOTween.To(
            () => _timer,
            SetCursor,
            1f,
            Difficulty.duration
        ).SetEase(Ease.Linear);
    }

    private void StopQte() =>
        _tween?.Kill();

    private void SetCursor(float value)
    {
        _timer = value;

        float difficultyDuration = Difficulty.duration / 2;

        _cursor.anchorMin = new Vector2(_timer, difficultyDuration);
        _cursor.anchorMax = new Vector2(_timer, difficultyDuration);
    }

    public void Evaluate()
    {
        StopQte();

        if (_timer >= Difficulty.PerfectStart && _timer <= Difficulty.PerfectEnd)
        {
            Perfect?.Invoke();
            return;
        }

        if (_timer >= Difficulty.OkStart && _timer <= Difficulty.OkEnd)
        {
            Ok?.Invoke();
            return;
        }

        Fail?.Invoke();
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }

    private void OnValidate()
    {
        if (_okZoneImage != null && _perfectZoneImage != null)
            SetupZones();

        if (_cursor != null)
            SetCursor(_timer);
    }

    private void SetupZones()
    {
        SetZone(_okZoneImage.rectTransform, Difficulty.OkStart, Difficulty.OkEnd);
        SetZone(_perfectZoneImage.rectTransform, Difficulty.PerfectStart, Difficulty.PerfectEnd);
    }

    private void SetZone(RectTransform zone, float start, float end)
    {
        zone.SetParent(_bar, false);
        zone.anchorMin = new Vector2(start, 0f);
        zone.anchorMax = new Vector2(end, 1f);
        zone.offsetMin = Vector2.zero;
        zone.offsetMax = Vector2.zero;
    }
}