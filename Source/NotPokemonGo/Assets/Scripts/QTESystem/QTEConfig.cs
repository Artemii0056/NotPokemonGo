using System.Collections.Generic;
using UnityEngine;

namespace QTESystem
{
    [CreateAssetMenu(fileName = nameof(QTEConfig), menuName = "StaticData/" + nameof(QTEConfig))]
    public class QTEConfig : ScriptableObject
    {
        public QTEType QTEType;
        public List<QTESetup> QteSetup;
    }

    public enum QTEType
    {
        Unit = 1,
        UI = 2
    }
}