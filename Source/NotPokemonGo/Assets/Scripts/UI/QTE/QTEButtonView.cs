using System;
using Units;
using UnityEngine;

namespace UI.QTE
{
    public abstract class QTEButtonView : MonoBehaviour
    {
        public float CurrentTime { get; protected set; }

        public abstract event Action<QTEButtonView> Successed;
        public abstract event Action<QTEButtonView> Invalided;

        protected Unit Unit;

        public virtual void Initialize(QTEPhasePresenter qtePhasePresenter)
        { }

        public virtual void Construct(Unit unit)
        {
            Unit = unit;
        }
    }
}