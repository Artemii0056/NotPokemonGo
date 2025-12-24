using System;
using UI.QTE;
using Units;

namespace QTESystem
{
    public interface IQteService
    {
        void Start(QteType qteType, Unit target);
        (QteButtonView,QtePhasePresenter) PlaySimple(QteType qteType, Unit target);
        event Action <bool> Completed;
    }
}