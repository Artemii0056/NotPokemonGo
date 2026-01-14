using System;
using QTESystem.TestQTE;
using Units;

namespace QTESystem
{
    public interface IQteService
    {
        void Start(QteType qteType, Unit target);
        TimingBarQte PlayTimingBar(QteType qteType, Unit target, float duration);
        IQteSession StartSession(QteType qteType, Unit target, float duration);
        event Action <bool> Completed;
    }
}