using System;
using System.Collections;
using QTESystem;
using Services;
using UnityEngine;
using UnityEngine.Pool;

namespace UI.QTE
{
    public class QTESpawner
    {
        private readonly ObjectPool<QTEButtonView>  _qtePool;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly int _allQteCount;
        private readonly QTEConfig _qteConfig;
        private readonly QTEPresenter1 _qtePresenter1;
        
        private Coroutine _spawnCoroutine;
        
        public event Action Finished;
        
        public QTESpawner(ICoroutineRunner coroutineRunner, QTEConfig qteConfig, QTEPresenter1 qtePresenter1)
        {
            _qteConfig = qteConfig;
            _qtePresenter1 = qtePresenter1;
            _allQteCount = qteConfig.QteSetup.Count;
            
            QTESetup firstQTESetup  = _qteConfig.QteSetup[0] ?? throw new ArgumentNullException(nameof(qteConfig.QteSetup));

            QTECanvas qteCanvas = GameObject.Instantiate(qteConfig.QteCanvas);
            _qtePresenter1.AddPanel(qteCanvas.QTEBacgroundPanelPrefab);

            _qtePool = new ObjectPool<QTEButtonView>
            (
                () => GameObject.Instantiate(firstQTESetup.QTEButtonView, qteCanvas.transform),
                ActionOnGet,
                (qteButton) => qteButton.gameObject.SetActive(false),
                (qteButton) => GameObject.Destroy(qteCanvas.gameObject)
                );

            _coroutineRunner = coroutineRunner;
        }

        public void Spawn()
        {
            _spawnCoroutine = _coroutineRunner.StartCoroutine(StartSpawn());
        }
        
        private void ActionOnGet(QTEButtonView qteButton)
        {
            qteButton.gameObject.SetActive(true);
            qteButton.Released += ReleaseQTEButton;
        }

        private void ReleaseQTEButton(QTEButtonView qteButton)
        {
            qteButton.Released -= ReleaseQTEButton;
            _qtePool.Release(qteButton);
        }
        
        private IEnumerator StartSpawn()
        {
            for (int i = 0; i < _allQteCount; i++)
            {
                Vector2 position = UnityEngine.Random.insideUnitCircle * 300f;
                
                QTEButtonView qteButtonView = _qtePool.Get();
                
                qteButtonView.Initialize(_qteConfig.QteSetup[i].Offset, _qteConfig.QteSetup[i].TargetTime, position);
                
                _qtePresenter1.AddView(qteButtonView);
                
                if (i == _allQteCount - 1)
                {
                    Finished?.Invoke();
                    break;
                }
                
                yield return new WaitForSeconds(_qteConfig.QteSetup[i].TimeToNextTarget);
            }
        }

        public void StopSpawn()
        {
            if (_spawnCoroutine != null) 
                _coroutineRunner.StopCoroutine(_spawnCoroutine);   
            
            Clear();
        }
        
        private void Clear()
        {
            _qtePool.Clear();
        }
    }
}