using System;
using QteSystem.Core;
using UI.QTE;
using Object = UnityEngine.Object;

namespace QteSystem.Runtime
{
    public sealed class QteViewSession : IQteSession
    {
        private readonly QteButtonView _view;
        private readonly IProvidesQteResult _resultProvider;
        private readonly bool _useExplicitResultProvider;

        private bool _disposed;

        public event Action<QteResult> Completed;

        public bool IsCompleted { get; private set; }
        public QteResult? Result { get; private set; }

        public QteViewSession(QteButtonView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _resultProvider = view as IProvidesQteResult;
            _useExplicitResultProvider = _resultProvider != null;

            if (_useExplicitResultProvider)
            {
                _resultProvider.OnReached += OnResulted;
            }
            else
            {
                _view.Successed += OnSuccessed;
                _view.Invalided += OnInvalided;
            }
        }

        private void OnResulted(QteResult result)
        {
            Complete(result);
        }

        private void OnSuccessed(QteButtonView _)
        {
            Complete(QteResult.Normal);
        }

        private void OnInvalided(QteButtonView _)
        {
            Complete(QteResult.Fail);
        }

        private void Complete(QteResult result)
        {
            if (_disposed || IsCompleted)
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

            if (_useExplicitResultProvider)
            {
                _resultProvider.OnReached -= OnResulted;
            }
            else
            {
                _view.Successed -= OnSuccessed;
                _view.Invalided -= OnInvalided;
            }

            if (_view != null)
                Object.Destroy(_view.gameObject);
        }
    }
}