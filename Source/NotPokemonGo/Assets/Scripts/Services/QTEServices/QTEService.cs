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
        private readonly QTEPresenter _qtePresenter;

        public event Action <bool> Completed; 
        
        public QTEService(IStaticDataService staticDataService, ICoroutineRunner coroutineRunner)
        {
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
            _qtePresenter = new QTEPresenter();
        }
        
        public void Start()
        {
            QTEConfig qteConfig = _staticDataService.GetQTEConfig(QTEType.UI);
            // перенести камеру в презенторе или в другом классе
            
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