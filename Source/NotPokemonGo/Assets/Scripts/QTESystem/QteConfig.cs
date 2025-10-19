using System.Collections.Generic;
using Abilities;
using UI.QTE;
using UnityEngine;
using UnityEngine.Serialization;

namespace QTESystem
{
    [CreateAssetMenu(fileName = nameof(QteConfig), menuName = "StaticData/" + nameof(QteConfig))]
    public class QteConfig : ScriptableObject
    {
       // public AbilityType AbilityType;
       public QteType QteType;
        public List<QtePhaseSetup> QtePhaseSetups;
        public QTECanvas QteCanvas;
    }
}