using System;
using QTESystem;
using Services;

namespace UI.QTE
{
    public class QTEPresenter
    {
        public event Action<bool> Completed;

        public virtual void Enable(QTEConfig qteConfig, ICoroutineRunner coroutineRunner)
        {
        }

        public virtual void Disable()
        {
            
        }
    }
}