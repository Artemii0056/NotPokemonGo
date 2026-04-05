using System;
using QteSystem.Core;
using UI.QTE;

namespace QteSystem.Views.Base
{
    public abstract class QteInteractiveView : QteButtonView, IProvidesQteResult
    {
        private bool _completed;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;
        public event Action<QteResult> OnReached;

        protected void Complete(QteResult result)
        {
            if (_completed)
                return;

            _completed = true;

            switch (result)
            {
                case QteResult.Fail:
                    Invalided?.Invoke(this);
                    break;

                case QteResult.Normal:
                case QteResult.Perfect:
                    Successed?.Invoke(this);
                    break;
            }

            OnReached?.Invoke(result);
        }
    }
}