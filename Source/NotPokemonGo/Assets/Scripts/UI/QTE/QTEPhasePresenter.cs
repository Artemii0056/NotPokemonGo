using Abilities;
using QTESystem;
using UnityEngine;
using VContainer;

namespace UI.QTE
{
  public class QTEPhasePresenter
  {
    private readonly IObjectResolver _objectResolver;
    private readonly QTEButtonView _qteButtonView;
    private bool _isActive;

    public QTEPhasePresenter(QTEPhaseSetup qtePhaseSetup, QTEButtonView qteButtonView, AbilityType abilityType)
    {
      _qteButtonView = qteButtonView;
      QtePhaseSetup = qtePhaseSetup;
      AbilityType = abilityType;
    }

    public QTEPhaseSetup QtePhaseSetup { get; }
    public AbilityType AbilityType { get; }

    public bool IsSuccess { get; private set; }

    public void Enable()
    {
      _isActive = true;

      _qteButtonView.Initialize(this);
      _qteButtonView.Successed += OnSuccessed;
      _qteButtonView.Invalided += OnInvalided;
    }

    public void Disable()
    {
      _qteButtonView.Successed -= OnSuccessed;
      _qteButtonView.Invalided -= OnInvalided;
    }

    public bool IsActive() =>
      _isActive;

    private void OnInvalided(QTEButtonView qteButtonView)
    {
      qteButtonView.Invalided -= OnInvalided;
      _isActive = false;
      IsSuccess = false;
    }
    
    private void OnSuccessed(QTEButtonView qteButtonView)
    {
      qteButtonView.Successed -= OnSuccessed;
      Debug.Log("ура");
      _isActive = false;
      IsSuccess = true;
    }
  }
}