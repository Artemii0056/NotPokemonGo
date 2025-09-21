using System;
using QTESystem;

namespace UI.QTE
{
  public class QTEPhasePresenter
  {
    private bool _isProceeded;
    private QTEButtonView _qteButtonView;

    public bool IsSuccess { get; private set; }

    public void Enable(QTEPhaseSetup qtePhaseSetup)
    {
      _isProceeded = true;

      QTESpawer qteSpawer = new QTESpawer();

      switch (qtePhaseSetup.QTEPhaseType)
      {
        case QTEPhaseType.ТапатьПоВрагу:
          _qteButtonView = qteSpawer.Spawn(qtePhaseSetup.QTEButtonView);
          Subscribe();
          break;

        case QTEPhaseType.ТапатьПоUI:
          Subscribe();
          break;

        case QTEPhaseType.ПереместитьЦельПоКанвасу:
          _qteButtonView = qteSpawer.Spawn(qtePhaseSetup.QTEButtonView);
          Subscribe();
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void Disable()
    {
      _qteButtonView.Successed -= OnSuccessed;
      _qteButtonView.Invalided -= OnInvalided;
    }

    public bool IsProceeded() =>
      _isProceeded;

    private void OnInvalided(QTEButtonView qteButtonView)
    {
      qteButtonView.Invalided -= OnInvalided;
      _isProceeded = false;
      IsSuccess = false;
    }

    private void Subscribe()
    {
      _qteButtonView.Successed += OnSuccessed;
      _qteButtonView.Invalided += OnInvalided;
    }

    private void OnSuccessed(QTEButtonView qteButtonView)
    {
      qteButtonView.Successed -= OnSuccessed;
      _isProceeded = false;
      IsSuccess = true;
    }
  }
}