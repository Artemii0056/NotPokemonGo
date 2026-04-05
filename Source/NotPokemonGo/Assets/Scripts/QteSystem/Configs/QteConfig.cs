using QteSystem.Core;
using UI.QTE;
using UnityEngine;

namespace QteSystem.Configs
{
    [CreateAssetMenu(fileName = nameof(QteConfig), menuName = "Config/" + nameof(QteConfig))]
    public class QteConfig : ScriptableObject 
    {
        public QteType QteType;
        public QteButtonView QteButtonView;
    }
}