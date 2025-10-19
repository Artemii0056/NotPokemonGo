using System;
using System.Collections.Generic;
using System.Linq;
using Services.InputServices;
using Services.RaycastServices;
using Services.StaticDataServices;
using Units;
using UnityEngine;
using VContainer;

namespace UI.QTE
{
  public class QTERaycasterView : QteButtonView
  {
    private IRaycastService _raycastService;
    private ITargetSelector _targetSelector;
    private IInputReader _inputReader;
    private QtePhasePresenter _qtePhasePresenter;
    private List<Unit> _units;
    private int _clickCount;
    private IStaticDataService _staticDataService;

    public override event Action<QteButtonView> Successed;
    public override event Action<QteButtonView> Invalided;
    
    [Inject]
    public void Construct(IInputReader inputReader, IRaycastService raycastService, ITargetSelector targetSelector, IStaticDataService staticDataService)
    {
      _staticDataService = staticDataService;
      _inputReader = inputReader;
      _targetSelector = targetSelector;
      _raycastService = raycastService;
      _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
    }

    public override void Initialize(QtePhasePresenter qtePhasePresenter)
    {
      base.Initialize(qtePhasePresenter);
      _qtePhasePresenter = qtePhasePresenter;
      
      _units = _targetSelector.GetTargets(_staticDataService.GetTargetMode(qtePhasePresenter.AbilityType)).Where(x => x != null).ToList();

      foreach (var unit in _units)
      {
        if (unit.TryGetComponent(out ColorGradient _) == false)
        {
          ColorGradient colorGradient = unit.gameObject.AddComponent<ColorGradient>();

          colorGradient.MarkProcess();
        }
      }
    }

    private void Update()
    {
      CurrentTime += Time.deltaTime * _qtePhasePresenter.QtePhaseSetup.Speed;

      if (CurrentTime >= _qtePhasePresenter.QtePhaseSetup.TargetTime)
      {
        Invalided?.Invoke(this);
      }
    }

    private void OnLeftMouseButtonClicked()
    {
      if (_raycastService.Raycast(out Unit unit) == false)
      {
        Invalided?.Invoke(this);
        return;
      }
      
      Debug.Log("OnLeftMouseButtonClicked - clickCount = " + _clickCount);
      Debug.LogWarning(_units.Count);
      
      for (int i = 0; i < _units.Count; i++)
      {
        _units[i].GetComponent<ColorGradient>().MarkProcess();

        if (_units[i] == unit)
        {
          Debug.LogWarning(_units[i].name);
          _clickCount++;
          _units[i].GetComponent<ColorGradient>().MarkInterract();
        }
        else
        {
          Invalided?.Invoke(this);
        }
      }

      if (_clickCount == _qtePhasePresenter.QtePhaseSetup.ClickCount)
      {
        Successed?.Invoke(this);
      }
    }

    private void OnDestroy() =>
      _inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonClicked;
  }
}