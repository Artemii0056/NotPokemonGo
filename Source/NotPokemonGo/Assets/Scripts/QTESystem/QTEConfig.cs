using System.Collections.Generic;
using Abilities;
using UI.QTE;
using UnityEngine;

namespace QTESystem
{
    [CreateAssetMenu(fileName = nameof(QTEConfig), menuName = "StaticData/" + nameof(QTEConfig))]
    public class QTEConfig : ScriptableObject
    {
        public AbilityType AbilityType;
        //public QTEType QTEType;
        public List<QTEPhaseSetup> QtePhaseSetups;
        public QTECanvas QteCanvas;
    }
}