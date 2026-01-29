using System;
using QTESystem.TestQTE;
using UI.QTE;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QTESystem
{
    public sealed class QteViewSession : IQteSession
    {
        private readonly QteButtonView _view;
        private readonly IProvidesQteResult _resultProvider;

        private bool _disposed;
    
        public event Action<QteResult> Completed;

        public bool IsCompleted { get; private set; }
        public QteResult? Result { get; private set; }

        public QteViewSession(QteButtonView view)
        {
            _view = view;

            _view.Successed += OnSuccessed;
            _view.Invalided += OnInvalided;

            _resultProvider = view as IProvidesQteResult;
            
            if (_resultProvider != null)
                _resultProvider.OnReached += OnResulted;
        }

        private void OnResulted(QteResult result)
        {
            Debug.Log("QteViewSession.OnResulted");
            
            // Продвинутый QTE сам сказал точный результат
            Complete(result);
        }

        private void OnSuccessed(QteButtonView _)
        {
            // Базовый QTE: успех => Normal
            // (Если конкретный QTE умеет Perfect — он должен вызвать Resulted(Perfect),
            // и мы сюда вообще не обязаны полагаться.)
            Complete(QteResult.Normal);
        }

        private void OnInvalided(QteButtonView _)
        {
            Complete(QteResult.Fail);
        }

        private void Complete(QteResult result)
        {
            if (IsCompleted) 
                return;

            IsCompleted = true;
            Result = result;
            Completed?.Invoke(result);
        }

        public void Dispose()
        {
            if (_disposed) 
                return;
            
            _disposed = true;

            if (_resultProvider != null)
                _resultProvider.OnReached -= OnResulted;

            if (_view != null)
            {
                _view.Successed -= OnSuccessed;
                _view.Invalided -= OnInvalided;

                Object.Destroy(_view.gameObject);
            }
        }
    }
}
