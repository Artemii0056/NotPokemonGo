using System;
using Units;
using UnityEngine;

namespace UI.QTE
{
    public abstract class QteButtonView : MonoBehaviour
    {
        public float CurrentTime { get; protected set; }

        public abstract event Action<QteButtonView> Successed;
        public abstract event Action<QteButtonView> Invalided;

        protected Unit Unit;

        public virtual void Initialize(QtePhasePresenter qtePhasePresenter)
        { }

        public virtual void Construct(Unit unit)
        {
            Unit = unit;
        }
    }
}