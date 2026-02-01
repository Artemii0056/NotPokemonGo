using System;
using TimeServices;
using Units;
using UnityEngine;

namespace UI.QTE
{
    public abstract class QteButtonView : MonoBehaviour
    {
        public abstract event Action<QteButtonView> Successed;
        public abstract event Action<QteButtonView> Invalided;

        protected Unit Unit;
        protected ITimeService TimeService;

        public virtual void Construct(Unit unit, ITimeService timeService)
        {
            Unit = unit;
            TimeService = timeService;
        }
    }
}