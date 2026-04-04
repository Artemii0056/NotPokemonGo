using System.Collections.Generic;
using AbilityNew.Scripts.Presentation;
using UnityEngine;

namespace AbilityNew.Scripts.Executors
{
    [CreateAssetMenu(menuName = "Ability/Presentation/Ability Presentation Config")]
    public sealed class AbilityPresentationConfig : ScriptableObject
    {
        public AbilitySo Ability;
        public List<PresentationEntry> Entries = new();
    }
}