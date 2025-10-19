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

        public QteButtonView Spawn(QteButtonView prefabQteButtonView)
        {
            QteButtonView qteButtonView = _objectResolver.Instantiate(prefabQteButtonView);
            return qteButtonView;
        }
    }
}