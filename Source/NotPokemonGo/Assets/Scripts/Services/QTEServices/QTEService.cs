using System;
using System.Collections;
using Abilities;
using QTESystem;
using Services.StaticDataServices;
using UI.QTE;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.QTEServices
{
    public class QTEService : IQTEService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IObjectResolver _objectResolver;

        public event Action <bool> Completed; 
        
        public QTEService(IStaticDataService staticDataService, ICoroutineRunner coroutineRunner, IObjectResolver objectResolver)
        {
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
            _objectResolver = objectResolver;
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
                QTEButtonView qteButtonView = _objectResolver.Instantiate(qtePhaseSetup.QTEButtonView);
                QTEPhasePresenter qtePhasePresenter = new QTEPhasePresenter(qtePhaseSetup, qteButtonView);

                qtePhasePresenter.Enable();
                
                yield return new WaitWhile(qtePhasePresenter.IsActive);

                qtePhasePresenter.Disable();

                if (qtePhasePresenter.IsSuccess == false)
                {
                    Completed?.Invoke(false);
                    yield break;
                }
            }
            
            Debug.Log("Stop foreach QTE");
            Completed?.Invoke(true);
        }
    }
}