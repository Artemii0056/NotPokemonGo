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

    public override event Action<QTEButtonView> Successed;
    public override event Action<QTEButtonView> Invalided;

    [Inject]
    public void Construct(IInputReader inputReader, IRaycastService raycastService, ITargetSelector targetSelector)
    {
      _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
      _inputReader = inputReader;
      _targetSelector = targetSelector;
      _raycastService = raycastService;
    }

    private void OnLeftMouseButtonClicked()
    {
      if (_raycastService.Raycast(out Unit unit) == false)
        Invalided?.Invoke(this);

      List<Unit> units = _targetSelector.GetTargets(TargetMode.Single);

      foreach (Unit unitInList in units)
      {
        if (unitInList == unit)
        {
          Successed?.Invoke(this);
          break;
        }
      }
    }

    private void OnDestroy() =>
      _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
  }
}