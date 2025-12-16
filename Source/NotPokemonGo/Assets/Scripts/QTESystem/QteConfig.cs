using System.Collections.Generic;
using UI.QTE;
using UnityEngine;

namespace QTESystem
{
    [CreateAssetMenu(fileName = nameof(QteConfig), menuName = "Config/" + nameof(QteConfig))]
    public class QteConfig : ScriptableObject //Отрефакторить
    {
        public QteType QteType;
        public List<QtePhaseSetup> QtePhaseSetups;
        public QTECanvas QteCanvas;
    }
}