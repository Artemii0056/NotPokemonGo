using System;
using System.Collections;
using Services;
using Services.StaticDataServices;
using TimeServices;
using UI.QTE;
using Units;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace QTESystem
{
    public class QteService : IQteService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IObjectResolver _objectResolver;
        private readonly ITimeService _timeService;

        public event Action<bool> Completed;

        public QteService(
            IStaticDataService staticDataService,
            ICoroutineRunner coroutineRunner,
            IObjectResolver objectResolver,
            ITimeService timeService)
        {
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
            _objectResolver = objectResolver;
            _timeService = timeService;
        }

        public void Start(QteType qteType, Unit target)
        {
            QteConfig qteConfig = _staticDataService.GetQteConfig(qteType);

            _coroutineRunner.StartCoroutine(PlayQte(qteConfig, target));
        }

        private IEnumerator PlayQte(QteConfig qteConfig, Unit target) //TODO Передавать время работы QTE? 
        {
            QteButtonView view = Object.Instantiate(qteConfig.QteButtonView);

            view.Construct(target, _timeService);
            _objectResolver.Inject(view);
            QtePhasePresenter qtePhasePresenter = new QtePhasePresenter(view);
            qtePhasePresenter.Enable();

            yield return new WaitWhile(qtePhasePresenter.IsActive);
            Object.Destroy(view.gameObject);
            qtePhasePresenter.Disable();

            if (qtePhasePresenter.IsSuccess == false)
            {
                Completed?.Invoke(false);
                yield break;
            }

            Completed?.Invoke(true);
        }
        
        public IQteSession StartSession(QteType qteType, Unit target, float duration)
        {
            QteConfig qteConfig = _staticDataService.GetQteConfig(qteType);

            QteButtonView view = Object.Instantiate(qteConfig.QteButtonView);

            view.Construct(target, _timeService);
            _objectResolver.Inject(view);

            if (view is IHasQteDuration durationView)
                durationView.SetDuration(duration);

            return new QteViewSession(view);
        }
    }
}