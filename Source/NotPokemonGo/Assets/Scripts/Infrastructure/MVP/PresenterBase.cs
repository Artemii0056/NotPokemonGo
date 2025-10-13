using Infrastructure.MVP.Implementation;

namespace Infrastructure.MVP
{
    public class PresenterBase : IPresenter
    {
        private readonly IView _view;
        private readonly Model _model;

        public PresenterBase(IView view, Model model)
        {
            _view = view;
            _model = model;
        }
        
        public void Enable()
        {
            _view.Show();
        }

        public void Disable()
        {
            _view.Hide();
        }
    }
}