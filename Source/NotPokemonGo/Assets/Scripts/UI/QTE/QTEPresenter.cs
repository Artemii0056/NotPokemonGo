using System;
using System.Collections.Generic;
using QTESystem;
using Services;
using UnityEngine;

namespace UI.QTE
{
    public class QTEPresenter
    {
        private QTESpawner _qteSpawner;
        public event Action<bool> Completed;

        private Queue<QTEButtonView> _qteButtonViews = new Queue<QTEButtonView>();
        private QTEBacgroundPanel _panel;

        public void Enable(QTEConfig qteConfig, ICoroutineRunner coroutineRunner)
        {
            _qteSpawner = new QTESpawner(coroutineRunner, qteConfig, this);
            _qteSpawner.Spawn();
            _qteSpawner.Finished += SpawnFinished;
        }

        public void Disable()
        {
            _qteButtonViews.Clear();
            _panel.Clicked -= StopQte;
            _qteSpawner.Finished -= SpawnFinished;
            GameObject.Destroy(_panel);
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

        private void SpawnFinished() => 
            Completed?.Invoke(true);

        private void OnInvalided(QTEButtonView qteButtonView)
        {
            qteButtonView.Invalided -= OnInvalided;
            StopQte();
        }

        private void OnSuccessed(QTEButtonView qteButtonView)
        {
            qteButtonView.Successed -= OnSuccessed;

            if (qteButtonView != _qteButtonViews.Dequeue())
                StopQte();
            else
                qteButtonView.ReleaseToPool();
        }

        private void StopQte()
        {
            foreach (QTEButtonView buttonView in _qteButtonViews) 
                buttonView.ReleaseToPool();

            _qteSpawner.StopSpawn();
            Completed?.Invoke(false);
        }
    }
}