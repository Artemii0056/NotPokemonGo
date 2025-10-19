using Abilities;
using QTESystem;
using UnityEngine;
using VContainer;

namespace UI.QTE
{
  public class QtePhasePresenter
  {
    private readonly IObjectResolver _objectResolver;
    private readonly QteButtonView _qteButtonView;
    private bool _isActive;

    public QtePhasePresenter(QtePhaseSetup qtePhaseSetup, QteButtonView qteButtonView, AbilityType abilityType)
    {
      _qteButtonView = qteButtonView;
      QtePhaseSetup = qtePhaseSetup;
      //AbilityType = abilityType;
      AbilityType = AbilityType.Defailt; //
    }

    public QtePhaseSetup QtePhaseSetup { get; }
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

    private void OnInvalided(QteButtonView qteButtonView)
    {
      qteButtonView.Invalided -= OnInvalided;
      _isActive = false;
      IsSuccess = false;
    }
    
    private void OnSuccessed(QteButtonView qteButtonView)
    {
      qteButtonView.Successed -= OnSuccessed;
      Debug.Log("ура");
      _isActive = false;
      IsSuccess = true;
    }
  }
}