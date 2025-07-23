using System;
using QTESystem;
using Services.StaticDataServices;
using UI.QTE;

namespace Services.QTEServices
{
    public class QTEService : IQTEService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;

        public event Action <bool> Completed; 
        
        private QTEPresenter _qtePresenter;
        public QTEService(IStaticDataService staticDataService, ICoroutineRunner coroutineRunner)
        {
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
        }
        
        public void Start()
        {
            QTEConfig qteConfig = _staticDataService.GetQTEConfig(QTEType.UI);
            // перенести камеру в презенторе или в другом классе

            switch (qteConfig.QTEType)
            {
                case QTEType.Sequential:
                    _qtePresenter = new QTEPresenter1();
                    break;
                
                case QTEType.Random:
                    _qtePresenter = new QTEPresenter2();
                    break;

                default:
                    throw new ArgumentOutOfRangeException("QTEType not found");
            }
            
            _qtePresenter.Enable(qteConfig, _coroutineRunner);

            _qtePresenter.Completed += OnCompleted;
        }

        private void OnCompleted(bool success)
        {
            _qtePresenter.Completed -= OnCompleted;

            _qtePresenter.Disable();

            Completed?.Invoke(success);
        }
    }
}