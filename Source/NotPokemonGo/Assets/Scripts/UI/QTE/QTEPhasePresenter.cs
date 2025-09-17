using System;
using QTESystem;

namespace UI.QTE
{
    public class QTEPhasePresenter
    {
        public QTEPhasePresenter(QTEPhaseSetup qtePhaseSetup)
        {
            switch (qtePhaseSetup.QTEPhaseType)
            {
                case QTEPhaseType.ТапатьПоВрагу:
                    // получить врага
                    break;
                
                case QTEPhaseType.ТапатьПоUI:
                    // заспавнить UI
                    break;
                
                case QTEPhaseType.ПереместитьЦельПоКанвасу:
                    // заспавнить UI
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsProceeded()
        {
            return false;
        }
    }
}