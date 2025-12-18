using UI.QTE;
using UnityEngine;

namespace QTESystem
{
    [CreateAssetMenu(fileName = nameof(QteConfig), menuName = "Config/" + nameof(QteConfig))]
    public class QteConfig : ScriptableObject 
    {
        public QteType QteType;
        public QteButtonView QteButtonView;
    }
}