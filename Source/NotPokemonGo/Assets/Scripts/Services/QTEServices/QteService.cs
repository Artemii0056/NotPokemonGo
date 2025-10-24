using System;
using System.Collections;
using Abilities;
using QTESystem;
using Services.StaticDataServices;
using UI.QTE;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Services.QTEServices
{
    public class QteService : IQteService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IObjectResolver _objectResolver;
        private readonly ISourceProvider _sourceProvider;
        private AbilityType _abilityType;

        public event Action <bool> Completed; 
        
        public QteService(
            IStaticDataService staticDataService, 
            ICoroutineRunner coroutineRunner, 
            IObjectResolver objectResolver, 
            ISourceProvider sourceProvider)
        {
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
            _objectResolver = objectResolver;
            _sourceProvider = sourceProvider;
        }
        
        public void Start(QteType qteType)
        {
            QteConfig qteConfig = _staticDataService.GetQteConfig(qteType);

            _coroutineRunner.StartCoroutine(StartQte(qteConfig));
        }

        private IEnumerator StartQte(QteConfig qteConfig)
        {
            foreach (QtePhaseSetup qtePhaseSetup in qteConfig.QtePhaseSetups)
            {
                QteButtonView view = GameObject.Instantiate(qtePhaseSetup.QTEButtonView);
                
                view.Construct(_sourceProvider.Source);
                _objectResolver.Inject(view);
                QtePhasePresenter qtePhasePresenter = new QtePhasePresenter(qtePhaseSetup, view, _abilityType);
                qtePhasePresenter.Enable();
                
                yield return new WaitWhile(qtePhasePresenter.IsActive);
                Object.Destroy(view.gameObject);
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