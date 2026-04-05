using Services.StaticDataServices;
using TimeServices;
using UI.QTE;
using VContainer;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QteSystem
{
    public sealed class QteService : IQteService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IObjectResolver _objectResolver;
        private readonly ITimeService _timeService;

        public QteService(
            IStaticDataService staticDataService,
            IObjectResolver objectResolver,
            ITimeService timeService)
        {
            _staticDataService = staticDataService;
            _objectResolver = objectResolver;
            _timeService = timeService;
        }

        public IQteSession StartSession(QteRequest request)
        {
            QteConfig qteConfig = _staticDataService.GetQteConfig(request.Type);

            QteButtonView view = Object.Instantiate(qteConfig.QteButtonView);
            view.Construct(request.Target, _timeService);

            _objectResolver.Inject(view);

            if (view is IHasQteDuration durationAware)
                durationAware.SetDuration(request.Duration);

            var outcomeAware = view as IHasQteOutcomeMode;
            
            if (outcomeAware != null)
                outcomeAware.SetOutcomeMode(request.OutcomeMode);

            return new QteViewSession(view);
        }
    }
}