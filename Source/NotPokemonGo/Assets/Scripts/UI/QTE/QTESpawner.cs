using System;
using System.Collections;
using System.Collections.Generic;
using QTESystem;
using UnityEngine;
using UnityEngine.Pool;

namespace UI.QTE
{
    public class QTESpawner
    {
        public event Action Failed;
        private ObjectPool<QTEButtonView> _pool;

        public QTESpawner()
        {
            // _pool = new ObjectPool<QTEButtonView>(
            //     GameObject.Instantiate(setup.QTEButtonView, spawnParent),
            //     ActionOnGet,
            //     ActionOnRelease);
        }

        private void ActionOnRelease(QTEButtonView obj)
        {
            
        }

        private void ActionOnGet(QTEButtonView obj)
        {
            
        }

        public IEnumerator Spawn(List<QTESetup> qtePrefab, Transform spawnParent)
        {
            foreach (QTESetup setup in qtePrefab)
            {
                QTEButtonView qteButtonView = GameObject.Instantiate(setup.QTEButtonView, spawnParent);
                Vector2 position = UnityEngine.Random.insideUnitCircle * 300f;
                qteButtonView.Initialize(setup.Offset, setup.TargetTime, position);
                QTEPresenter qtePresenter = new QTEPresenter(qteButtonView);
                
                
                yield return new WaitForSeconds(setup.TimeToNextTarget);
            }
        }

        private void ButtonSuccessed(bool isSuccess)
        {
            if (isSuccess == false) 
                Failed?.Invoke();
        }
    }
}