using System;
using QTESystem;

namespace UI.QTE
{
    public class QTEPhasePresenter
    {
        private bool _isProceeded;

        public QTEPhasePresenter(QTEPhaseSetup qtePhaseSetup)
        {
            _isProceeded = true;

            QTESpawer qteSpawer = new QTESpawer();

            switch (qtePhaseSetup.QTEPhaseType)
            {
                case QTEPhaseType.ТапатьПоВрагу:
                    // получить врага
                    break;

                case QTEPhaseType.ТапатьПоUI:
                    QTEButtonView qteButtonView = qteSpawer.Spawn(qtePhaseSetup.QTEButtonView);
                    qteButtonView.Successed += OnSuccessed;
                    qteButtonView.Invalided += OnInvalided;
                    break;

                case QTEPhaseType.ПереместитьЦельПоКанвасу:
                    // заспавнить UI
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsSuccess { get; private set; }

        public bool IsProceeded() =>
            _isProceeded;

        private void OnInvalided(QTEButtonView qteButtonView)
        {
            qteButtonView.Invalided -= OnInvalided;
            _isProceeded = false;
            IsSuccess = false;
        }

        private void OnSuccessed(QTEButtonView qteButtonView)
        {
            qteButtonView.Successed -= OnSuccessed;
            _isProceeded = false;
            IsSuccess = true;
        }
    }
}