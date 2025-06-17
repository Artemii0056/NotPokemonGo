using Infrastructure.MVP;
using Infrastructure.MVP.Implementation;

namespace Units
{
    public class UnitPresenter : PresenterBase
    {
        private readonly IView _view;
        private readonly Model _model;

        public UnitPresenter(IView view, Model model) : base(view, model)
        {
            _view = view;
            _model = model;
        }
    }
}