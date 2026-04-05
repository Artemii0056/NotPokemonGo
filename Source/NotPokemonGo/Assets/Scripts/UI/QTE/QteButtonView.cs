using System;
using TimeServices;
using Units;
using UnityEngine;

namespace UI.QTE
{
    public abstract class QteButtonView : MonoBehaviour
    {
        protected Unit Target;
        protected ITimeService TimeService;

        public abstract event Action<QteButtonView> Successed;
        public abstract event Action<QteButtonView> Invalided;

        public virtual void Construct(Unit target, ITimeService timeService)
        {
            Target = target;
            TimeService = timeService;
        }
    }
}