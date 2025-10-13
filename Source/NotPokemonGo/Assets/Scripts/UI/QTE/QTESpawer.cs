using VContainer;
using VContainer.Unity;

namespace UI.QTE
{
    public class QTESpawer
    {
        private readonly IObjectResolver _objectResolver;

        public QTESpawer(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public QTEButtonView Spawn(QTEButtonView prefabQteButtonView)
        {
            QTEButtonView qteButtonView = _objectResolver.Instantiate(prefabQteButtonView);
            return qteButtonView;
        }
    }
}