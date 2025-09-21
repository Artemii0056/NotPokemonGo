using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
  public class QTETargetMovementOnCanvas : QTEButtonView
  {
    private const float MaxValue = 1f;
    
    [SerializeField] private Slider _slider;
    [SerializeField] private float _timer;
    [SerializeField] private float _speed;

    public override event Action<QTEButtonView> Successed;
    public override event Action<QTEButtonView> Invalided;

    private void OnEnable() => 
      _slider.onValueChanged.AddListener(OnSliderValueChanged);

    private void OnDisable() => 
      _slider.onValueChanged.AddListener(OnSliderValueChanged);

    private void OnSliderValueChanged(float value)
    {
      if (Mathf.Approximately(value, MaxValue))
      {
        Successed?.Invoke(this);
      }
    }

    private void Update()
    {
      if (_timer <= 0)
      {
        Invalided?.Invoke(this);
        Debug.Log($"Failed in QTETargetMovementOnCanvas");
      }
      else
      {
        float time = _timer -= (Time.deltaTime * _speed);
        Debug.Log($"current time in QTETargetMovementOnCanvas - {time}");
      }
    }
  }
}