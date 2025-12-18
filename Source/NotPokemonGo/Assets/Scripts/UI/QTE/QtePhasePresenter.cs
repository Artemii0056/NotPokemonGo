using VContainer;

namespace UI.QTE
{
    public class QtePhasePresenter
    {
        private readonly IObjectResolver _objectResolver;
        private readonly QteButtonView _qteButtonView;
        private bool _isActive;

        public QtePhasePresenter(QteButtonView qteButtonView) => 
            _qteButtonView = qteButtonView;

        public bool IsSuccess { get; private set; }

        public void Enable()
        {
            _isActive = true;

            _qteButtonView.Initialize(this);
            _qteButtonView.Successed += OnSuccessed;
            _qteButtonView.Invalided += OnInvalided;
        }

        public void Disable()
        {
            _qteButtonView.Successed -= OnSuccessed;
            _qteButtonView.Invalided -= OnInvalided;
        }

        public bool IsActive() =>
            _isActive;

        private void OnInvalided(QteButtonView qteButtonView)
        {
            qteButtonView.Invalided -= OnInvalided;
            _isActive = false;
            IsSuccess = false;
        }

        private void OnSuccessed(QteButtonView qteButtonView)
        {
            qteButtonView.Successed -= OnSuccessed;
            _isActive = false;
            IsSuccess = true;
        }
    }
}