using System;
using System.Collections.Generic;
using Abilities;
using Services.InputServices;
using Services.RaycastServices;
using Units;
using VContainer;

namespace UI.QTE
{
  public class QTERaycasterView : QTEButtonView
  {
    private IRaycastService _raycastService;
    private ITargetSelector _targetSelector;
    private IInputReader _inputReader;
    private QTEPhasePresenter _qtePhasePresenter;

    public override event Action<QTEButtonView> Successed;
    public override event Action<QTEButtonView> Invalided;

    [Inject]
    public void Construct(IInputReader inputReader, IRaycastService raycastService, ITargetSelector targetSelector)
    {
      _inputReader = inputReader;
      _targetSelector = targetSelector;
      _raycastService = raycastService;
      _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
    }

    public override void Initialize(QTEPhasePresenter qtePhasePresenter)
    {
      base.Initialize(qtePhasePresenter);
      _qtePhasePresenter = qtePhasePresenter;
    }

    private void OnLeftMouseButtonClicked()
    {
      if (_raycastService.Raycast(out Unit unit) == false)
        Invalided?.Invoke(this);

      List<Unit> units = _targetSelector.GetTargets(TargetMode.Single);

      for (int i = 0; i < _qtePhasePresenter.QtePhaseSetup.ClickCount; i++)
      {
        foreach (Unit unitInList in units)
        {
          if (unitInList == unit)
          {
            Successed?.Invoke(this);
            return;
          }
        }
      }
      
      throw new Exception("No units found");
    }

    private void OnDestroy() =>
      _inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonClicked;
  }
}