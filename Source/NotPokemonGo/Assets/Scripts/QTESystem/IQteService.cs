using System;
using UI.QTE;
using Units;

namespace QTESystem
{
    public interface IQteService
    {
        void Start(QteType qteType, Unit target);
        (QteButtonView,QtePhasePresenter) StartSimple(QteType qteType, Unit target);
        event Action <bool> Completed;
    }
}