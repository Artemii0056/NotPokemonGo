using System;
using UnityEngine;

namespace UI.QTE
{
    public abstract class QTEButtonView : MonoBehaviour
    {
        public abstract event Action<QTEButtonView> Successed;
        public abstract event Action<QTEButtonView> Invalided;

        public virtual void Initialize(QTEPhasePresenter qtePhasePresenter)
        { }
    }
}