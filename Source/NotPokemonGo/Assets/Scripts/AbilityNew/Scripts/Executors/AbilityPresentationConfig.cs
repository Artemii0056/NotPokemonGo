using System;
using System.Collections.Generic;
using AbilityNew.Presentation;
using AbilityNew.Scripts.Presentation;
using UnityEngine;

namespace AbilityNew.Scripts.Executors
{
    [CreateAssetMenu(menuName = "Ability/Presentation/Ability Presentation Config")]
    public sealed class AbilityPresentationConfig : ScriptableObject
    {
        public AbilitySO Ability;
        public List<PresentationEntry> Entries = new();
    }

    [Serializable]
    public sealed class PresentationEntry
    {
        public AbilityPresentationSignal Signal;
        public List<PresentationAction> Actions = new();
    }
}