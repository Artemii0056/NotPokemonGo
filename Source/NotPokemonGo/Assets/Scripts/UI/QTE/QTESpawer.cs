using UnityEngine;

namespace UI.QTE
{
    public class QTESpawer
    {
        public QTEButtonView Spawn(QTEButtonView prefabQteButtonView)
        {
            QTEButtonView qteButtonView = GameObject.Instantiate(prefabQteButtonView);
            return qteButtonView;
        }
    }
}