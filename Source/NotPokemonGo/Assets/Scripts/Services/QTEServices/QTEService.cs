using System;
using System.Collections;
using Abilities;
using QTESystem;
using Services.StaticDataServices;
using UI.QTE;
using UnityEngine;

namespace Services.QTEServices
{
    public class QTEService : IQTEService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;

        public event Action <bool> Completed; 
        
        public QTEService(IStaticDataService staticDataService, ICoroutineRunner coroutineRunner)
        {
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
        }
        
        public void Start(AbilityType abilityType)
        {
            QTEConfig qteConfig = _staticDataService.GetQTEConfig(abilityType);

            _coroutineRunner.StartCoroutine(StartQTE(qteConfig));
        }

        private IEnumerator StartQTE(QTEConfig qteConfig)
        {
            foreach (QTEPhaseSetup qtePhaseSetup in qteConfig.QtePhaseSetups)
            {
                QTEPhasePresenter qtePhasePresenter = new QTEPhasePresenter(qtePhaseSetup);
                yield return new WaitUntil(qtePhasePresenter.IsProceeded);

                if (qtePhasePresenter.IsProceeded() == false)
                {
                    Completed?.Invoke(false);
                    yield break;
                }
            }
            
            Completed?.Invoke(true);
        }
    }
}