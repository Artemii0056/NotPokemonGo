using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Services.InputServices;
using Services.RaycastServices;
using Units;
using UnityEngine;
using VContainer;

namespace UI.QTE
{
  public class QTERaycasterView : QTEButtonView
  {
    private IRaycastService _raycastService;
    private ITargetSelector _targetSelector;
    private IInputReader _inputReader;
    private QTEPhasePresenter _qtePhasePresenter;
    private List<Unit> _units;
    private int _clickCount;

    public override event Action<QTEButtonView> Successed;
    public override event Action<QTEButtonView> Invalided;
    
    [Inject]
    public void Construct(IInputReader inputReader, IRaycastService raycastService, ITargetSelector targetSelector)
    {
      _inputReader = inputReader;
      _targetSelector = targetSelector;
      _raycastService = raycastService;
      _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
      
      _units = _targetSelector.GetTargets(TargetMode.Single).Where(x => x != null).ToList();

      foreach (var unit in _units)
      {
        if (unit.TryGetComponent(out ColorGradient _) == false)
        {
          ColorGradient colorGradient = unit.gameObject.AddComponent<ColorGradient>();

          colorGradient.MarkProcess();
        }
      }
    }

    public override void Initialize(QTEPhasePresenter qtePhasePresenter)
    {
      base.Initialize(qtePhasePresenter);
      _qtePhasePresenter = qtePhasePresenter;
    }

    private void Update()
    {
      CurrentTime += Time.deltaTime * _qtePhasePresenter.QtePhaseSetup.Speed;

      if (CurrentTime >= _qtePhasePresenter.QtePhaseSetup.TargetTime)
      {
        Invalided?.Invoke(this);
        Debug.LogWarning("прошло время QTERaycasterView");
      }
    }

    private void OnLeftMouseButtonClicked()
    {
      if (_raycastService.Raycast(out Unit unit) == false)
      {
        Invalided?.Invoke(this);

        Debug.LogWarning("не попал в юнита QTERaycasterView");
        return;
      }

      for (int i = 0; i < _qtePhasePresenter.QtePhaseSetup.ClickCount; i++)
      {
        for (int j = _units.Count; j >= 0; j--)
        {
          _units[j].GetComponent<ColorGradient>().MarkProcess();

          if (_units[j] == unit)
          {
            _clickCount++;
            _units[j].GetComponent<ColorGradient>().MarkInterract();
          }
          else
          {
            Debug.LogWarning("не попал в юнита в for QTERaycasterView");
            Invalided?.Invoke(this);
          }
        }
      }

      if (_clickCount == _qtePhasePresenter.QtePhaseSetup.ClickCount) 
        Successed?.Invoke(this);
    }

    private void OnDestroy() =>
      _inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonClicked;
  }

  public class ColorGradient : MonoBehaviour
  {
    private Renderer _renderer;
    private Color _defaultColor;

    private void Awake()
    {
      _renderer = GetComponent<Renderer>();
      _defaultColor = _renderer.material.color;
    }

    public void MarkProcess()
    {
      _renderer.material.SetColor("_Color", Color.red);
    }
    
    public void MarkInterract()
    {
      _renderer.material.SetColor("_Color", Color.yellow);
    }

    public void FinalizeProcess()
    {
      _renderer.material.color = _defaultColor;
    }
  }
}