using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Statuses
{
    [CreateAssetMenu(fileName = nameof(StatusTypesConfig), menuName = "StaticData/" + nameof(StatusTypesConfig))]
    public class StatusTypesConfig : ScriptableObject
    {
        [SerializeField] private List<StatusTypeIcon> _statusTypes;
        
        public List<StatusTypeIcon> StatusTypes => _statusTypes.ToList();
    }

    [Serializable]
    public class StatusTypeIcon
    {
        public StatusType Type;
        public Sprite Icon;
    }
}