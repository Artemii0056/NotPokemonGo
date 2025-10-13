using System;
using System.Collections.Generic;
using QTESystem;
using Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.QTE
{
    public class QTEPresenter
    {
        private QTESpawner _qteSpawner;
        public event Action<bool> Completed;

        private Queue<QTEButtonView> _qteButtonViews = new Queue<QTEButtonView>();
        private QTEBacgroundPanel _panel;
        private bool _isSpawnFinished;

        public void Enable(QTEConfig qteConfig, ICoroutineRunner coroutineRunner)
        {
            _qteSpawner = new QTESpawner(coroutineRunner, qteConfig, this);
            _qteSpawner.Spawn();
            _qteSpawner.Finished += SpawnFinished;
            _isSpawnFinished = false;
        }

        public void Disable()
        {
            _qteButtonViews.Clear();
            _qteSpawner.StopSpawn();
            _panel.Clicked -= StopQte;
            _qteSpawner.Finished -= SpawnFinished;
        }

        public void AddView(QTEButtonView qteButtonView)
        {
            _qteButtonViews.Enqueue(qteButtonView);
            qteButtonView.Successed += OnSuccessed;
            qteButtonView.Invalided += OnInvalided;
        }

        public void AddPanel(QTEBacgroundPanel panel)
        {
            _panel = panel;
            _panel.Clicked += StopQte;
        }

        private void SpawnFinished()
        {
            _isSpawnFinished = true;
        }

        private void OnInvalided(QTEButtonView qteButtonView)
        {
            qteButtonView.Invalided -= OnInvalided;
            StopQte();
        }

        private void OnSuccessed(QTEButtonView qteButtonView)
        {
            qteButtonView.Successed -= OnSuccessed;

            QTEButtonView buttonView = _qteButtonViews.Dequeue();

            if (qteButtonView != buttonView)
            {
                StopQte();
            }
            else if (_isSpawnFinished && _qteButtonViews.Count == 0)
            {
                Completed?.Invoke(true);
                qteButtonView.ReleaseToPool();
            }
            else
            {
                qteButtonView.ReleaseToPool();
            }
        }

        private void StopQte()
        {
            foreach (QTEButtonView buttonView in _qteButtonViews) 
                buttonView.ReleaseToPool();

            Completed?.Invoke(false);
        }
    }
}