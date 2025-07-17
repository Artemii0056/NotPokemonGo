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

        public QTEService(IStaticDataService staticDataService, ICoroutineRunner coroutineRunner)
        {
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
        }
        
        public void Start(Battlefield unitActionPayload)
        {
            QTEConfig qteConfig = _staticDataService.GetQTEConfig(QTEMode.Single);
            // перенести камеру
            // включить UI с кнопками
            // QTEPresenter qtePresenter = new QTEPresenter(qteConfig.QteSetup,  _coroutineRunner);
            
            QTESpawner qteSpawner = new QTESpawner();

            _coroutineRunner.StartCoroutine(qteSpawner.Spawn(qteConfig.QteSetup, default));
        }

        private void OnCompleted()
        {
            
        }
        
        private void OnFailed()
        {
            
        }
    }
}