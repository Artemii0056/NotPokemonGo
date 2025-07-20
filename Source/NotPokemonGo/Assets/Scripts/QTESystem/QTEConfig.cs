using System.Collections.Generic;
using UI.QTE;
using UnityEngine;

namespace QTESystem
{
    [CreateAssetMenu(fileName = nameof(QTEConfig), menuName = "StaticData/" + nameof(QTEConfig))]
    public class QTEConfig : ScriptableObject
    {
        public QTEType QTEType;
        public List<QTESetup> QteSetup;
        public QTECanvas QteCanvas;
    }
}